
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.OrdersCarts.CreateOrderCart
{
    public sealed class CreateOrdersCardHandler : IRequestHandler<CreateOrdersCardRequest, CreateOrdersCardResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrdersCartRepository _ordersCart;
        private readonly IMapper _mapper;

        public CreateOrdersCardHandler(IUnitOfWork unitOfWork,
                                IOrdersCartRepository ordersCart,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _ordersCart = ordersCart;
            _mapper = mapper;
        }
        public async Task<CreateOrdersCardResponse> Handle(CreateOrdersCardRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _mapper.Map<OrdersCart>(request);
                await _ordersCart.CreateAsync(order);
                await _unitOfWork.Commit(cancellationToken);

                return _mapper.Map<CreateOrdersCardResponse>(order);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
