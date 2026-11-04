using System.Net.Http.Json;
using WebFrontEnd.Services.Users.Dto;

namespace WebFrontEnd.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Users";

        public UsersService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<List<UserListItemDto>?> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/list");
                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<List<UserListItemDto>>()
                       ?? new List<UserListItemDto>();
            }
            catch
            {
                return null;
            }
        }

        public Task<ServiceResponsePost> CreateAsync(UserFormDto user) =>
            SendAsync(HttpMethod.Post, BaseUrl, new
            {
                email = user.Email.Trim(),
                password = user.Password,
                firstName = user.FirstName.Trim(),
                lastName = user.LastName.Trim(),
                nuit = user.Nuit.Trim(),
                cardCode = user.CardCode.Trim(),
                phoneNumber = user.PhoneNumber?.Trim(),
                role = user.Role,
                twoFactorEnabled = user.TwoFactorEnabled
            });

        public Task<ServiceResponsePost> UpdateAsync(string id, UserFormDto user) =>
            SendAsync(HttpMethod.Put, $"{BaseUrl}/{Uri.EscapeDataString(id)}", new
            {
                firstName = user.FirstName.Trim(),
                lastName = user.LastName.Trim(),
                nuit = user.Nuit.Trim(),
                cardCode = user.CardCode.Trim(),
                phoneNumber = user.PhoneNumber?.Trim(),
                role = user.Role,
                twoFactorEnabled = user.TwoFactorEnabled
            });

        public Task<ServiceResponsePost> SetStatusAsync(string id, bool isActive) =>
            SendAsync(HttpMethod.Put, $"{BaseUrl}/{Uri.EscapeDataString(id)}/status", new { isActive });

        /// <summary>
        /// Sends the request and returns the API message (success or business error).
        /// An empty message means an unexpected failure; the page shows its own generic text.
        /// </summary>
        private async Task<ServiceResponsePost> SendAsync(HttpMethod method, string url, object body)
        {
            try
            {
                using var request = new HttpRequestMessage(method, url) { Content = JsonContent.Create(body) };
                using var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                ServiceResponsePost? result = null;
                try
                {
                    result = General.DeserialzeJsonString<ServiceResponsePost>(json);
                }
                catch
                {
                    // Not our JSON (empty body, 500 page, ...)
                }

                if (result is not null && !string.IsNullOrWhiteSpace(result.Message))
                {
                    result.Flag = response.IsSuccessStatusCode && result.Flag;
                    return result;
                }

                return new ServiceResponsePost { Flag = response.IsSuccessStatusCode };
            }
            catch
            {
                return new ServiceResponsePost { Flag = false };
            }
        }
    }
}
