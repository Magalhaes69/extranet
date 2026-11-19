using System.Net.Http.Json;
using System.Text.Json;

namespace WebFrontEnd.Services.Activity
{
    public class ActivityItemDto
    {
        public Guid Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ActivityPageDto
    {
        public int Total { get; set; }
        public List<ActivityItemDto> Items { get; set; } = new();
    }

    public interface IUserActivityService
    {
        /// <summary>Actividade do utilizador autenticado; null se não foi possível carregar.</summary>
        Task<ActivityPageDto?> GetAsync(int page, int pageSize);
    }

    public class UserActivityService : IUserActivityService
    {
        private readonly HttpClient _http;

        public UserActivityService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<ActivityPageDto?> GetAsync(int page, int pageSize)
        {
            try
            {
                var response = await _http.GetAsync($"api/client/activity?page={page}&pageSize={pageSize}");
                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<ActivityPageDto>()
                    : null;
            }
            catch (Exception ex) when (ex is HttpRequestException or JsonException)
            {
                return null;
            }
        }
    }
}
