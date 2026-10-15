using Application.Shared.Behavior;
using Domain.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;

namespace WebAPI.SAP_Helps
{
    public sealed class SapSessionHandler : DelegatingHandler
    {
        private readonly SapSettings _settings;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private int _session; // 0 = no session; increments on every login

        public SapSessionHandler(IOptions<SapSettings> options) => _settings = options.Value;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var path = request.RequestUri?.AbsolutePath ?? string.Empty;
            if (path.EndsWith("/Login", StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith("/Logout", StringComparison.OrdinalIgnoreCase))
                return await base.SendAsync(request, ct);

            var body = request.Content is null ? null : await request.Content.ReadAsByteArrayAsync(ct);
            var session = await EnsureSessionAsync(expired: 0, ct);

            var response = await base.SendAsync(request, ct);
            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            // Session expired or was logged out -> log in again once and retry
            response.Dispose();
            await EnsureSessionAsync(expired: session, ct);
            return await base.SendAsync(Clone(request, body), ct);
        }

        private async Task<int> EnsureSessionAsync(int expired, CancellationToken ct)
        {
            var current = Volatile.Read(ref _session);
            if (current != 0 && current != expired) return current;

            await _lock.WaitAsync(ct);
            try
            {
                if (_session != 0 && _session != expired) return _session;
                await LoginAsync(ct);
                return ++_session;
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task LoginAsync(CancellationToken ct)
        {
            var uri = new Uri(new Uri(_settings.BaseUrl.TrimEnd('/') + "/"), "Login");
            var json = JsonSerializer.Serialize(new { _settings.CompanyDB, _settings.UserName, _settings.Password });

            using var login = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using var response = await base.SendAsync(login, ct);
            if (!response.IsSuccessStatusCode)
                throw new SapB1Exception(0, "SAP B1 login failed: " + await response.Content.ReadAsStringAsync(ct),
                    (int)response.StatusCode);
        }

        private static HttpRequestMessage Clone(HttpRequestMessage request, byte[]? body)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri) { Version = request.Version };
            foreach (var h in request.Headers)
                clone.Headers.TryAddWithoutValidation(h.Key, h.Value);

            if (body is not null)
            {
                clone.Content = new ByteArrayContent(body);
                foreach (var h in request.Content!.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }
            return clone;
        }
    }
}
