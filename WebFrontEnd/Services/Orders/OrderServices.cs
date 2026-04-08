using System.Net.Http.Json;
using WebFrontEnd.Services.Orders.Models;

namespace WebFrontEnd.Services.Orders
{
    public class OrderServices : IOrderService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Orders";
        private List<OrderViewDto>? _cache;
        private DetailsOrdersDto? orders;
        public Action OrderAction { get; set; }

        public OrderServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
            
        }

        public async Task<List<OrderViewDto>> GetOrdersAsync(string cardCode, bool forceRefresh = false)
        {
            if (_cache is not null && !forceRefresh)
                return _cache;

            var response = await _httpClient.GetAsync($"{BaseUrl}/list?CardCode={cardCode}");

            if (!response.IsSuccessStatusCode)
                return new List<OrderViewDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<OrderViewDto>>()
                     ?? new List<OrderViewDto>();

            return _cache;
        }

        private ServiceResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return new ServiceResponse(false, "Error occured. Try again later...");
            return new ServiceResponse(true, null!);
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
           

            List<DocumentLines> newDocumentoL = new List<DocumentLines>();
            foreach (var line in order.DocumentLines)
            {
                if (line.ItemCode is not null) {
                    newDocumentoL.Add(new DocumentLines
                    {
                        ItemCode = line.ItemCode,
                        Quantity = line.Quantity,
                        UnitPrice = line.UnitPrice,
                        WarehouseCode = line.WarehouseCode,
                        LineTotal = line.LineTotal,
                        ItemDescription = line.ItemDescription,
                        //VatGroup = line.VatGroup,
                    });
                }
            }


            var newOrderViewDto = new OrdersCreate()
            {
               CardCode = order.CardCode,
               DocDueDate = order.DocDueDate ?? DateTime.Now,
               Referencia = order.Referencia ?? string.Empty,
               CardName = order.CardName,
               DocumentLines = newDocumentoL,
            };

            var response = await _httpClient.PostAsync(BaseUrl, General.GennerateStringContent(General.SerializeObject(newOrderViewDto)));


            var result = CheckResponsePost(response);

            if (!result.Flag) return result;

            var apiResponse = await RedContent(response);

            return General.DeserialzeJsonString<ServiceResponsePost>(apiResponse);
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
    }
}
