using System.Net.Http.Json;
using WebFrontEnd.Services.Invoices.Dto;


namespace WebFrontEnd.Services.Invoices
{
    public class InvoicesService : IInvoicesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Invoices";
        private List<GetAllInvoicesDto>? _cache;
        public Action InvoicesAction { get; set; }

        public InvoicesService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<List<GetAllInvoicesDto>> GetInvoicesAsync(string cardCode, bool forceRefresh = false)
        {
            if (_cache is not null && !forceRefresh)
                return _cache;

            var response = await _httpClient.GetAsync($"{BaseUrl}/list?CardCode={cardCode}");

            if (!response.IsSuccessStatusCode)
                return new List<GetAllInvoicesDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<GetAllInvoicesDto>>()
                     ?? new List<GetAllInvoicesDto>();

            return _cache;
        }
    }
}
