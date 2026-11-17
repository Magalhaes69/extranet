using WebFrontEnd.Services.Orders.Models;
using WebFrontEnd.Shared.Dtos;
using WebFrontEnd.Shared.Results;

namespace WebFrontEnd.Services.Orders
{
    public interface IOrderService
    {

        //Task GetOrdersAsync(string CardCode);
        Task<OrderDto> GetByIdAsync(int id);
        Task<ServiceResponsePost> CreateAsync(OrderDto order);
        Task<ServiceResponsePost> CreateCatalgAsync(OrdersCreate order);
        Task<PagedResult<OrderViewDto, TotalResultDto>> GetOrdersAsync(
            OrdersRequest ordersRequest, 
            bool forceRefresh = false);
        Task<DetailsOrdersDto> GetByDocEntryOrdersAsync(string cardCode, int DocEntry, bool forceRefresh = false);
        Action OrderAction { get; set; }
    }
}
