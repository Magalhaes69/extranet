using System.Net.Http.Json;
using System.Text.Json;
using WebFrontEnd.Services.Orders.Models;
using WebFrontEnd.Services.Quotations.Dto;
using WebFrontEnd.Shared.Dtos;
using WebFrontEnd.Shared.Results;

namespace WebFrontEnd.Services.Orders
{
    public class OrderServices : IOrderService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Orders";
        private readonly Dictionary<string, PagedResult<OrderViewDto, TotalResultDto>> _cache = new();
        private DetailsOrdersDto? orders;
        public Action OrderAction { get; set; }

        public OrderServices(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
            
        }

        public async Task<PagedResult<OrderViewDto, TotalResultDto>> GetOrdersAsync(OrdersRequest request, bool forceRefresh = false)
        {
            var body = General.SerializeObject(request);   // also used as the cache key

            if (!forceRefresh && _cache.TryGetValue(body, out var cached))
                return cached;

            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/list", General.GennerateStringContent(body));

                if (!response.IsSuccessStatusCode)
                    return PagedResult<OrderViewDto, TotalResultDto>.Empty(request.Page, request.PageSize);

                var result = await response.Content.ReadFromJsonAsync<PagedResult<OrderViewDto, TotalResultDto>>()
                             ?? PagedResult<OrderViewDto, TotalResultDto>.Empty(request.Page, request.PageSize);

                _cache[body] = result;
                return result;
            }
            catch
            {
                return PagedResult<OrderViewDto, TotalResultDto>.Empty(request.Page, request.PageSize);
            }
        }

        private ServiceResponsePost CheckResponsePost(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponsePost()
                {
                    Flag = false,
                    Message = "Error occured. Try again later..."
                };
            }

            return new ServiceResponsePost()
            {
                Flag = true,
                Message = null!
            };
        }



        public async Task<ServiceResponsePost> CreateAsync(OrderDto order)
        {
            var lines = order.DocumentLines
                .Where(l => !string.IsNullOrWhiteSpace(l.ItemCode))
                .Select(l => new DocumentLines
                {
                    ItemCode = l.ItemCode,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice,
                    WarehouseCode = l.WarehouseCode,
                    LineTotal = l.LineTotal,
                    ItemDescription = l.ItemDescription,
                    //VatGroup = l.VatGroup,
                })
                .ToList();

            if (lines.Count == 0)
                return new ServiceResponsePost { Flag = false, Message = "Add at least one item to the order." };

            var newOrder = new OrdersCreate
            {
                CardCode = order.CardCode,
                DocDueDate = order.DocDueDate ?? DateTime.Now,
                Referencia = order.Referencia ?? string.Empty,
                CardName = order.CardName ?? string.Empty,
                DocumentLines = lines,
            };

            try
            {
                var response = await _httpClient.PostAsync(
                    BaseUrl, General.GennerateStringContent(General.SerializeObject(newOrder)));

                var body = await RedContent(response);

                // The API returns ServiceResponse for both success and error
                if (!string.IsNullOrWhiteSpace(body))
                {
                    try
                    {
                        var result = General.DeserialzeJsonString<ServiceResponsePost>(body);
                        if (result is not null)
                            return result;
                    }
                    catch (JsonException)
                    {
                        // body wasn't a ServiceResponse (e.g. 401 or proxy error page)
                    }
                }

                return new ServiceResponsePost
                {
                    Flag = false,
                    Message = $"Request failed ({(int)response.StatusCode} {response.ReasonPhrase})."
                };
            }
            catch (HttpRequestException)
            {
                return new ServiceResponsePost { Flag = false, Message = "Could not reach the server." };
            }
        }

        private static async Task<string> RedContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();

        public async Task<DetailsOrdersDto> GetByDocEntryOrdersAsync(string cardCode, int DocEntry, bool forceRefresh = false)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/details?CardCode={cardCode}&DocEntry={DocEntry}");

            if ((!response.IsSuccessStatusCode))
            {
                return null!;
            }

            orders = await response.Content.ReadFromJsonAsync<DetailsOrdersDto>()
                     ?? new DetailsOrdersDto();

            return orders;
        }

        public async Task<ServiceResponsePost> CreateCatalgAsync(OrdersCreate order)
        {
            try
            {
                var response = await _httpClient.PostAsync(BaseUrl, General.GennerateStringContent(General.SerializeObject(order)));
                var result = CheckResponsePost(response);

                if (!result.Flag) return result;

                var apiResponse = await RedContent(response);

                return General.DeserialzeJsonString<ServiceResponsePost>(apiResponse);
            }
            catch
            {
                return new ServiceResponsePost()
                {
                    Flag = false,
                    Message = "Error occured. Try again later..."
                };
            }
        }
    }
}
