using AutoMapper;
using Domain.Models;


namespace Application.UseCases.OrdersCarts.CreateOrderCart
{
    public sealed class CreateOrdersCardMapper : Profile
    {
        public CreateOrdersCardMapper()
        {
            CreateMap<CreateOrdersCardRequest, OrdersCart>();
            CreateMap<CreateOrderLineCartRequest, OrderLineCart>();
            CreateMap<OrdersCart, CreateOrdersCardResponse>();
        }
    }
}
