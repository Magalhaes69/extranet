
using Application.UseCases.OrdersCarts.UpdateOrderCartLine;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.OrdersCarts.DeleteOrderCartLine
{
    public class DeleteOrderCartLineHandler : IRequestHandler<DeleteOrderCartLineRequest,List<DeleteOrderCartLineResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderLineCartRepository _ordersCart;
        private readonly IMapper _mapper;
        public DeleteOrderCartLineHandler(IUnitOfWork unitOfWork,
                                IOrderLineCartRepository ordersCart,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _ordersCart = ordersCart;
            _mapper = mapper;
        }
        public async Task<List<DeleteOrderCartLineResponse>> Handle(DeleteOrderCartLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _mapper.Map<OrderLineCart>(request);
                _ordersCart.DeleteAsync(order);
                await _unitOfWork.Commit(cancellationToken);
                var list = await _ordersCart.GetAll(request.OrderId);

                return _mapper.Map<List<DeleteOrderCartLineResponse>>(list);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
