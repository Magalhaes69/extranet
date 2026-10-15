using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.OrdersCarts.UpdateOrderCartLine
{
    public class UpdateOrderCartLineHandler : IRequestHandler<UpdateOrderCartLineRequest, UpdateOrderCartLineResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderLineCartRepository _ordersCart;
        private readonly IMapper _mapper;

        public UpdateOrderCartLineHandler(IUnitOfWork unitOfWork,
                                IOrderLineCartRepository ordersCart,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _ordersCart = ordersCart;
            _mapper = mapper;
        }
        public async Task<UpdateOrderCartLineResponse> Handle(UpdateOrderCartLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var getOrder = await _ordersCart.GetByIdAsync(request.OrderLineCartId, cancellationToken);
                if (getOrder is null)
                {
                    return null!;
                }
                
                getOrder.ItemCode = request.ItemCode;
                getOrder.Quantity  = request.Quantity;  
                getOrder.UnitPrice = request.UnitPrice;
                getOrder.WarehouseCode = request.WarehouseCode!;
                getOrder.ItemDescription = request.ItemDescription;
                getOrder.LineTotal = request.LineTotal;
                getOrder.DiscPrcnt = request.DiscPrcnt;
                getOrder.UserId = request.UserId;

                var order = _mapper.Map<OrderLineCart>(getOrder);
                _ordersCart.UpdateAsync(order);
                await _unitOfWork.Commit(cancellationToken);

                return _mapper.Map<UpdateOrderCartLineResponse>(order);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
