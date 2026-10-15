

using AutoMapper;
using Domain.Models;

namespace Application.UseCases.OrdersCarts.DeleteOrderCart
{
    public class DeleteOrderCartMapper : Profile
    {
        public DeleteOrderCartMapper()
        {
            CreateMap<DeleteOrderCartRequest, OrdersCart>();
            CreateMap<OrdersCart, DeleteOrderCartResponse>();
        }
    }
}
