using WebFrontEnd.Services.Orders.Models;
using WebFrontEnd.Services.OrdersCarts.Dto;

namespace WebFrontEnd.Services.OrdersCarts
{
    public interface IOrdersCartService
    {
        Task<ServiceResponsePost> CreateAsync(CreateOrdersCartDto order);
        Task<ServiceResponseGeneralPost<CreateOrderLineCartResponseDto>> AddLineAsync(CreateOrderLineCartExistOrderDto order);
        Task<ServiceResponseGeneralPost<UpdateOrdersCartLineDto>> UpdateLineAsync(UpdateOrdersCartLineDto order);
        Task<ServiceResponseGeneralPost<ViewOrderLineCartDto>> DeleteLineAsync(string OrderLineCartId, string OrderId);

        Task<ViewOrdersCartDto> GetLastOrdersCartPend(string CardCode, string UserId, bool forceRefresh = false);
        Action OrderAction { get; set; }
    }
}
