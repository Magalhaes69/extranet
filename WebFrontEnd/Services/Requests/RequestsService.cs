using System.Net.Http.Json;
using WebFrontEnd.Services.Requests.Dtos;

namespace WebFrontEnd.Services.Requests
{
    public class RequestsService : IRequestsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/client/requests";

        public RequestsService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<List<RequestListItemDto>> GetAllRequestsAsync(CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(BaseUrl, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"Get requests failed ({(int)response.StatusCode}): {body}");
                    return new List<RequestListItemDto>();
                }

                return await response.Content.ReadFromJsonAsync<List<RequestListItemDto>>(cancellationToken: ct)
                    ?? new List<RequestListItemDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar solicitações: {ex.Message}");
                return new List<RequestListItemDto>();
            }
        }

        public async Task<RequestDetailsDto?> GetRequestDetailsAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", ct);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"Get request details failed ({(int)response.StatusCode}): {body}");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<RequestDetailsDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar detalhes da solicitação: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateRequestAsync(CreateRequestDto dto, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"Create request failed ({(int)response.StatusCode}): {body}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar solicitação: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CancelRequestAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", ct);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"Cancel request failed ({(int)response.StatusCode}): {body}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cancelar solicitação: {ex.Message}");
                return false;
            }
        }
    }
}
