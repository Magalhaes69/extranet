

using AutoMapper;
using Domain.Models;

namespace Application.UseCases.OrdersCarts.DeleteOrderCartLine
{
    public class DeleteOrderCartLineMapper : Profile
    {
        public DeleteOrderCartLineMapper()
        {
            CreateMap<DeleteOrderCartLineRequest, OrderLineCart>();
            CreateMap<OrderLineCart, DeleteOrderCartLineResponse>();
        }
    }
}
