using System.Net.Http.Json;
using WebFrontEnd.Services.IncomingPayments.Dto;
using WebFrontEnd.Services.Returns.Dto;

namespace WebFrontEnd.Services.Returns
{
    public class ReturnsService : IReturnsService
    {
        public Action IncomingPaymentsServiceAction { get; set; }

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Returns";
        private GetAllReturnsResultDto _cache;


        public ReturnsService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public Action ReturnsServiceAction { get; set; }

        public Task<ViewDetailsIncomingPaymentsDto> GetByDocNum(GetByDocNumIncomingPaymentsRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<GetAllReturnsResultDto> GetAllAsync(GetAllReturnsRequestDto request, bool forceRefresh = false)
        {
            try
            {
                if (_cache is not null && !forceRefresh)
                    return _cache;

                request.CardCode = string.Empty;

                var serialized = General.GennerateStringContent(General.SerializeObject(request));
                var response = await _httpClient.PostAsync($"{BaseUrl}/list", serialized);

                if (!response.IsSuccessStatusCode)
                    return new GetAllReturnsResultDto();

                _cache = await response.Content.ReadFromJsonAsync<GetAllReturnsResultDto>()
                         ?? new GetAllReturnsResultDto();

                return _cache;
            }
            catch
            {
                return new GetAllReturnsResultDto();
            }
        }
    }
}
