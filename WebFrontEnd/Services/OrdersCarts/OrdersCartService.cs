using System.Net.Http.Json;
using WebFrontEnd.Services.OrdersCarts.Dto;

namespace WebFrontEnd.Services.OrdersCarts
{
    public class OrdersCartService : IOrdersCartService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/OrdersCart";
        public ViewOrdersCartDto CurrentCart { get; private set; } = new();
        public Action OrderAction { get; set; }
        public event Action? OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();
        public OrdersCartService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }

        public async Task<ServiceResponsePost> CreateAsync(CreateOrdersCartDto order)
        {
            var response = await _httpClient.PostAsync(BaseUrl,
                General.GennerateStringContent(General.SerializeObject(order)));

            var result = CheckResponsePost(response);

            if (!result.Flag) return result;

            var apiResponse = await RedContent(response);

            var created = General.DeserialzeJsonString<ServiceResponsePost>(apiResponse);

           
            await GetLastOrdersCartPend(order.CardCode, order.UserId, true);

            return created;
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
        private static async Task<string> RedContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();
        public async Task<ViewOrdersCartDto> GetLastOrdersCartPend(string CardCode, string UserId, bool forceRefresh = false)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/pendent?CardCode={CardCode}&UserId={UserId}");

                if (!response.IsSuccessStatusCode)
                    return CurrentCart;

                CurrentCart = await response.Content.ReadFromJsonAsync<ViewOrdersCartDto>()
                              ?? new ViewOrdersCartDto();

                NotifyStateChanged();

                return CurrentCart;
            }
            catch
            {
                return CurrentCart;
            }
        }

        public async Task<ServiceResponseGeneralPost<UpdateOrdersCartLineDto>> UpdateLineAsync(UpdateOrdersCartLineDto order)
        {
            try
            {
                var response = await _httpClient.PutAsync(
                BaseUrl,General.GennerateStringContent(General.SerializeObject(order)));

                var result = CheckResponsePost(response);

                if (!result.Flag)
                    return new ServiceResponseGeneralPost<UpdateOrdersCartLineDto>
                    {
                        Flag = false,
                        Message = result.Message
                    };

                var apiResponse = await RedContent(response);

                var created = General.DeserialzeJsonString<ServiceResponseGeneralPost<UpdateOrdersCartLineDto>>(apiResponse);

                return created;
            }
            catch(Exception ex)
            {
                return new ServiceResponseGeneralPost<UpdateOrdersCartLineDto>
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponseGeneralPost<CreateOrderLineCartResponseDto>> AddLineAsync(CreateOrderLineCartExistOrderDto order)
        {
            try
            {
                order.LineTotal = order.Quantity * order.UnitPrice;
                var response = await _httpClient.PostAsync($"{BaseUrl}/addnew",
                   General.GennerateStringContent(General.SerializeObject(order)));

                var result = CheckResponsePost(response);

                if (!result.Flag)
                    return new ServiceResponseGeneralPost<CreateOrderLineCartResponseDto>
                    {
                        Flag = false,
                        Message = result.Message
                    };

                var apiResponse = await RedContent(response);

                var created = General.DeserialzeJsonString<ServiceResponseGeneralPost<CreateOrderLineCartResponseDto>>(apiResponse);

                return created;
                
            }
            catch (Exception ex)
            {
                return new ServiceResponseGeneralPost<CreateOrderLineCartResponseDto>
                {
                    Flag = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponseGeneralPost<ViewOrderLineCartDto>> DeleteLineAsync(string OrderLineCartId, string OrderId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/deleteline?OrderLineCartId={OrderLineCartId}&OrderId={OrderId}");

                if (!response.IsSuccessStatusCode)
                    return new ServiceResponseGeneralPost<ViewOrderLineCartDto>
                    {
                        Flag = false,
                        Message = ""
                    };


                var apiResponse = await RedContent(response);

                var result = General.DeserialzeJsonString<ServiceResponseGeneralPost<ViewOrderLineCartDto>>(apiResponse);

                

                if(result is null)
                    return new ServiceResponseGeneralPost<ViewOrderLineCartDto>
                    {
                        Flag = false,
                        Message = ""
                    };

                if (result.Flag)
                {
                    CurrentCart.OrderLineCarts = result.Datalist ?? new List<ViewOrderLineCartDto>();
                    NotifyStateChanged();
                }

               

               return result;
            }
            catch
            {
                return new ServiceResponseGeneralPost<ViewOrderLineCartDto>
                {
                    Flag = false,
                    Message = ""
                };
            }
        }
    }
}
