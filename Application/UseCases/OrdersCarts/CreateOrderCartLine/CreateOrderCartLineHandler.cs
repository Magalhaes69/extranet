
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.OrdersCarts.CreateOrderCartLine
{
    public class CreateOrderCartLineHandler : IRequestHandler<CreateOrderCartLineRequest, CreateOrderCartLineResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderLineCartRepository _ordersCart;
        private readonly IMapper _mapper;

        public CreateOrderCartLineHandler(IUnitOfWork unitOfWork,
                                IOrderLineCartRepository ordersCart,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _ordersCart = ordersCart;
            _mapper = mapper;
        }
        public async Task<CreateOrderCartLineResponse> Handle(CreateOrderCartLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _mapper.Map<OrderLineCart>(request);
                _ordersCart.CreateAsync(order);
                await _unitOfWork.Commit(cancellationToken);

                return _mapper.Map<CreateOrderCartLineResponse>(order);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
