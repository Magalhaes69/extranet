
using Application.UseCases.OrdersCarts.CreateOrderCart;
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.OrdersCarts.UpdateOrdersCarts
{
    public class UpdateOrdersCartsMapper : Profile
    {
        public UpdateOrdersCartsMapper()
        {
            CreateMap<UpdateOrdersCartsRequest, OrdersCart>();
            CreateMap<OrdersCart, UpdateOrdersCardResponse>();
        }
    }
}
