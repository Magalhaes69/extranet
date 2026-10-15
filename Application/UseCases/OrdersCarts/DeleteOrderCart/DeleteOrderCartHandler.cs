using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.OrdersCarts.DeleteOrderCart
{
    public sealed class DeleteOrderCartHandler : IRequestHandler<DeleteOrderCartRequest, DeleteOrderCartResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrdersCartRepository _repository;
        private readonly IMapper _mapper;

        public DeleteOrderCartHandler(
            IUnitOfWork unitOfWork,
            IOrdersCartRepository repository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<DeleteOrderCartResponse> Handle(DeleteOrderCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var getOrder = await _repository.GetByIdAsync(
                    request.OrderId, 
                    cancellationToken);

                if (getOrder == null)
                    return null!;

                var order = _mapper.Map<OrdersCart>(getOrder);
                _repository.DeleteAsync(order);
                await _unitOfWork.Commit(cancellationToken);
                return _mapper.Map<DeleteOrderCartResponse>(order);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
