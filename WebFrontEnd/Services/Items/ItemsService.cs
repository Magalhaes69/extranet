using System.Net.Http.Json;
using WebFrontEnd.Services.Items.DTO;

namespace WebFrontEnd.Services.Items
{
    public class ItemsService : IItemsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Items";
        public Action ItemsAction { get; set; }
        private List<ItemsDto>? _cache;
        private List<CatalogItemDto>? _catalogCache;

        public ItemsService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public async Task<List<ItemsDto>> GetItemsAsync()
        {
            if (_cache is not null)
                return _cache;

            var valid = "tYES";
            var queryParams = new List<string>();
            queryParams.Add($"Valid={valid}");

            var query = string.Join("&", queryParams);
            var response = await _httpClient.GetAsync($"{BaseUrl}/list?{query}");

            if (!response.IsSuccessStatusCode)
                return new List<ItemsDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<ItemsDto>>()
                     ?? new List<ItemsDto>();

            return _cache;
        }

        public  async Task<List<CatalogItemDto>> GetFirstPageAsync(GetAllitemsCatalogRequest request)
        {
            try
            {
                var serialized = General.GennerateStringContent(General.SerializeObject(request));
                var response = await _httpClient.PostAsync($"{BaseUrl}/catalog", serialized);

                if (!response.IsSuccessStatusCode)
                    return new List<CatalogItemDto>();

                _catalogCache = await response.Content.ReadFromJsonAsync<List<CatalogItemDto>>()
                         ?? new List<CatalogItemDto>();

                return _catalogCache;
            }
            catch
            {
                return new List<CatalogItemDto>();
            }
        }

        public async Task GetBackgroundAsync(
         GetAllitemsCatalogRequest request,
         Action<List<CatalogItemDto>> onPageLoaded,
         CancellationToken cancellationToken)
        {
            int page = 2;

            while (!cancellationToken.IsCancellationRequested)
            {
                var pagedRequest = new GetAllitemsCatalogRequest
                {
                    Page = page,
                    PageSize = request.PageSize,
                    Valid = request.Valid,
                    GroupNumbes = request.GroupNumbes?.ToList() ?? new List<int>()
                };

                var serialized = General.GennerateStringContent(
                    General.SerializeObject(pagedRequest));

                var response = await _httpClient.PostAsync($"{BaseUrl}/catalog", serialized);

                if (!response.IsSuccessStatusCode)
                    break;

                var data = await response.Content.ReadFromJsonAsync<List<CatalogItemDto>>()
                           ?? new List<CatalogItemDto>();

                if (data.Count == 0)
                    break;

                onPageLoaded?.Invoke(data);

                if (data.Count < request.PageSize)
                    break;

                page++;
            }
        }
    }
}
