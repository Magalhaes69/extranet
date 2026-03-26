using System.Net.Http.Json;
using WebFrontEnd.Services.IncomingPayments.Dto;
using WebFrontEnd.Services.Invoices.Dto;

namespace WebFrontEnd.Services.IncomingPayments
{
    public class IncomingPaymentsService : IIncomingPaymentsService
    {
        public Action IncomingPaymentsServiceAction { get; set; }

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/IncomingPayments";
        private List<GetAllIncomingPaymentsDto>? _cache;
        public Action InvoicesAction { get; set; }

        public IncomingPaymentsService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<List<GetAllIncomingPaymentsDto>> GetIncomingPaymentsServiceAsync(string cardCode, bool forceRefresh = false)
        {
            if (_cache is not null && !forceRefresh)
                return _cache;

            var response = await _httpClient.GetAsync($"{BaseUrl}/list?CardCode={cardCode}");

            if (!response.IsSuccessStatusCode)
                return new List<GetAllIncomingPaymentsDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<GetAllIncomingPaymentsDto>>()
                     ?? new List<GetAllIncomingPaymentsDto>();

            return _cache;
        }
    }
}
