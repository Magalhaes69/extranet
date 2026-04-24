
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.OrdersCarts.CreateOrderCartLine
{
    public class CreateOrderCartLineMapper : Profile
    {
        public CreateOrderCartLineMapper()
        {
            CreateMap<CreateOrderCartLineRequest, OrderLineCart>();
            CreateMap<OrderLineCart, CreateOrderCartLineResponse>();
        }
    }
}
