using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace WebFrontEnd.Services.Auth
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly NavigationManager _nav;
        private readonly ILocalStorageService _localStorage;

        public AuthHandler(NavigationManager nav, ILocalStorageService localStorage)
        {
            _nav = nav;
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.GetItemAsync<string>("access");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _localStorage.RemoveItemAsync("access");
                _nav.NavigateTo($"/login?returnUrl={Uri.EscapeDataString(_nav.Uri)}", true);
            }

            return response;
        }
    }
}
