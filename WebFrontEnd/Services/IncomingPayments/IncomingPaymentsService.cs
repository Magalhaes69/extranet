using System.Net.Http.Json;
using WebFrontEnd.Services.IncomingPayments.Dto;
using WebFrontEnd.Shared.Dtos;


namespace WebFrontEnd.Services.IncomingPayments
{
    public class IncomingPaymentsService : IIncomingPaymentsService
    {
        public Action IncomingPaymentsServiceAction { get; set; }

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/IncomingPayments";
        private GetIncomingPaymentsResult _cache;
      

        public IncomingPaymentsService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }

        public async Task<GetIncomingPaymentsResult> GetIncomingPaymentsServiceAsync(IncomingPaymentsRequest request, bool forceRefresh = false)
        {
            try
            {
                if (_cache is not null && !forceRefresh)
                    return _cache;

                request.CardCode = string.Empty;

                var serialized = General.GennerateStringContent(General.SerializeObject(request));
                var response = await _httpClient.PostAsync($"{BaseUrl}/list", serialized);

                if (!response.IsSuccessStatusCode)
                    return new GetIncomingPaymentsResult();

                _cache = await response.Content.ReadFromJsonAsync<GetIncomingPaymentsResult>()
                         ?? new GetIncomingPaymentsResult();

                return _cache;
            }
            catch
            {
                return new GetIncomingPaymentsResult();
            }
        }

        public async Task<ViewDetailsIncomingPaymentsDto> GetByDocNum(
            GetByDocNumIncomingPaymentsRequest request)
        {
            try
            {

                var serialized = General.GennerateStringContent(General.SerializeObject(request));
                var response = await _httpClient.PostAsync($"{BaseUrl}/details", serialized);

                if (!response.IsSuccessStatusCode)
                    return new ViewDetailsIncomingPaymentsDto();

                var result = await response.Content.ReadFromJsonAsync<ViewDetailsIncomingPaymentsDto>()
                         ?? new ViewDetailsIncomingPaymentsDto();

                return result;
            }
            catch
            {
                return new ViewDetailsIncomingPaymentsDto();
            }
        }
    }
}
