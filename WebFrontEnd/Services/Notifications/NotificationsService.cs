using System.Net.Http.Json;
using System.Text.Json;

namespace WebFrontEnd.Services.Notifications
{
    public class NotificationItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        /// <summary>Info, Warning, Success ou Error.</summary>
        public string Level { get; set; } = "Info";
        public bool IsRead { get; set; }
        public string? ActionUrl { get; set; }
        public string? ActionText { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class NotificationsDto
    {
        public int Unread { get; set; }
        public List<NotificationItemDto> Items { get; set; } = new();
    }

    public interface INotificationsService
    {
        /// <summary>null = não foi possível carregar (mantém o estado anterior).</summary>
        Task<NotificationsDto?> GetAsync();
        Task MarkReadAsync(Guid id);
        Task MarkAllReadAsync();
    }

    public class NotificationsService : INotificationsService
    {
        private const string BaseUrl = "api/client/notifications";
        private readonly HttpClient _http;

        public NotificationsService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<NotificationsDto?> GetAsync()
        {
            try
            {
                var response = await _http.GetAsync($"{BaseUrl}?take=30");
                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<NotificationsDto>()
                    : null;
            }
            catch (Exception ex) when (ex is HttpRequestException or JsonException)
            {
                return null;
            }
        }

        public Task MarkReadAsync(Guid id) => PostAsync($"{BaseUrl}/{id}/mark-read");

        public Task MarkAllReadAsync() => PostAsync($"{BaseUrl}/mark-all-read");

        private async Task PostAsync(string url)
        {
            try
            {
                await _http.PostAsync(url, null);
            }
            catch (HttpRequestException)
            {
                // Marcar como lida é secundário: se falhar, a próxima actualização repõe o estado real.
            }
        }
    }
}
