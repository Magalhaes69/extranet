
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.OrdersCarts.GetLastOrderCartPend
{
    public sealed class GetLastOrdersCartPendHandler : IRequestHandler<GetLastOrdersCartPendRequest, GetLastOrdersCartPendResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOrdersCartRepository _repository;
        public GetLastOrdersCartPendHandler(IMapper mapper, IOrdersCartRepository repository)
        {
            this._mapper = mapper;
            this._repository = repository;
        }
        public async Task<GetLastOrdersCartPendResponse> Handle(GetLastOrdersCartPendRequest request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetLastOrderPendAsync(request.CardCode, request.UserId, cancellationToken);
            
            return _mapper.Map<GetLastOrdersCartPendResponse>(orders);
        }
    }
}
