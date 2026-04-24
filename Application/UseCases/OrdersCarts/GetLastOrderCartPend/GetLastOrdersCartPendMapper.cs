using AutoMapper;
using Domain.Models;

namespace Application.UseCases.OrdersCarts.GetLastOrderCartPend
{
    public sealed class GetLastOrdersCartPendMapper : Profile
    {
        public GetLastOrdersCartPendMapper()
        {
            CreateMap<OrdersCart, GetLastOrdersCartPendResponse>();
            CreateMap<OrderLineCart, GetLastOrderLineCartPendResponse>();

        }
    }
}
