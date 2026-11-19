using System.Net.Http.Json;
using WebFrontEnd.Services.ItemGroups.Dto;


namespace WebFrontEnd.Services.ItemGroups
{
    public class ItemGroupsService : IItemGroupsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/ItemGroups";
        private List<ItemGroupsDto>? _cache;
        public Action ItemGroupsDto { get; set; }

        public ItemGroupsService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }
        public async Task<List<ItemGroupsDto>> GetItemsGroupsAsync()
        {
            if (_cache is not null)
                return _cache;
            var response = await _httpClient.GetAsync($"{BaseUrl}/list");

            if (!response.IsSuccessStatusCode)
                return new List<ItemGroupsDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<ItemGroupsDto>>()
                     ?? new List<ItemGroupsDto>();

            return _cache;
        }
    }
}
