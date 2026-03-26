using WebFrontEnd.Services.Orders.Models;

namespace WebFrontEnd.Services.Orders
{
    public interface IOrderService
    {

        //Task GetOrdersAsync(string CardCode);
        Task<OrderDto> GetByIdAsync(int id);
        Task<ServiceResponsePost> CreateAsync(OrderDto order);

        Task<List<OrderViewDto>> GetOrdersAsync(string cardCode, bool forceRefresh = false);

        Action OrderAction { get; set; }
    }
}
