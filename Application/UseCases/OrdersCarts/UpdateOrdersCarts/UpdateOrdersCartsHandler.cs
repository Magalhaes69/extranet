

using Application.Dtos.Orders;
using Application.UseCases.Orders.CreateOrder;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.OrdersCarts.UpdateOrdersCarts
{
    public sealed class UpdateOrdersCartsHandler : IRequestHandler<UpdateOrdersCartsRequest, UpdateOrdersCardResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOrdersCartRepository _repository;
        private IUnitOfWork _unitOfWork;
        private readonly ISapClient _sapclient;

        public UpdateOrdersCartsHandler(
            IMapper mapper,
            IOrdersCartRepository repository,
            IUnitOfWork unitOfWork,
            ISapClient sapClient)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _sapclient = sapClient;
        }
        public async Task<UpdateOrdersCardResponse> Handle(UpdateOrdersCartsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var getOrder = await _repository.GetByOrderIdAsync(
                    request.OrderId, 
                    cancellationToken);

                if (getOrder == null)
                    return null!;

                var orders = new CreateB1OrderRequest()
                {
                    CardCode = getOrder.CardCode,
                    DocDueDate = DateTime.UtcNow,
                    Referencia = getOrder.Referencia!,
                    CardName = getOrder.CardName,
                };

                var docLines = new List<OrderLineDto>();

                foreach (var item in getOrder.OrderLineCarts)
                {
                    docLines.Add(new OrderLineDto()
                    {
                        ItemCode = item.ItemCode,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        WarehouseCode = item.WarehouseCode,
                        ItemDescription = item.ItemDescription!,
                        LineTotal = item.LineTotal,
                        DiscountPercent = item.DiscPrcnt,
                        VatGroup = item.VatGroup,
                    });
                }
                orders.DocumentLines = docLines;

                var login = await _sapclient.LoginAsync(cancellationToken);
                if (!login) throw new Exception("SAP b1 login failde");

                var results = await _sapclient.AddAsync<SapOrderDto>(
                    "Orders",
                    orders,
                    cancellationToken);

                getOrder.Status = request.Status;
                getOrder.UserId = request.UserId;
                var order = _mapper.Map<OrdersCart>(getOrder);
                _repository.UpdateAsync(order);
                await _unitOfWork.Commit(cancellationToken);

                return _mapper.Map<UpdateOrdersCardResponse>(order);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
