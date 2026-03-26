using System.Net.Http.Json;
using WebFrontEnd.Services.Items.DTO;
using WebFrontEnd.Services.Orders.Models;

namespace WebFrontEnd.Services.Items
{
    public class ItemsService : IItemsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Items";
        public Action ItemsAction { get; set; }
        private List<ItemsDto>? _cache;

        public ItemsService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public Task<ItemsDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemsDto>> GetItemsAsync()
        {
            if (_cache is not null)
                return _cache;

            var response = await _httpClient.GetAsync($"{BaseUrl}/list?");

            if (!response.IsSuccessStatusCode)
                return new List<ItemsDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<ItemsDto>>()
                     ?? new List<ItemsDto>();

            return _cache;
        }
    }
}
