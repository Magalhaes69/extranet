using AutoMapper;
using Domain.Models;

namespace Application.UseCases.OrdersCarts.UpdateOrderCartLine
{
    public sealed class UpdateOrderCartLineMapper : Profile
    {
        public UpdateOrderCartLineMapper()
        {
            CreateMap<UpdateOrderCartLineRequest, OrderLineCart>();
            CreateMap<OrderLineCart, UpdateOrderCartLineResponse>();
        }
    }
}
