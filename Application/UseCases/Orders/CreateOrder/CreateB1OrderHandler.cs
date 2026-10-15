using Application.Dtos.Orders;
using Application.Shared.Behavior;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateB1OrderHandler : IRequestHandler<CreateB1OrderRequest, CreateOrderResponse>
    {
        private readonly ISapClient _sapClient;
        public CreateB1OrderHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<CreateOrderResponse> Handle(CreateB1OrderRequest request, CancellationToken cancellationToken)
        {
            var login = await _sapClient.LoginAsync(cancellationToken);
            if (!login) throw new SapB1Exception(0, "SAP B1 login failed", 401);

            try
            {
                var result = await _sapClient.AddAsync<SapOrderDto>("Orders", request, cancellationToken);

                return new CreateOrderResponse
                {
                    DocEntry = result.DocEntry,
                    DocNum = result.DocNum,
                    DocType = result.DocType,
                    DocumentLines = (result.DocumentLines ?? new()).Select(x => new CreateOrderLineResponse
                    {
                        LineNum = x.LineNum,
                        ItemCode = x.ItemCode,
                        Quantity = x.Quantity ?? 0,
                        UnitPrice = x.UnitPrice ?? 0
                    }).ToList()
                };
            }
            finally
            {
                await _sapClient.LogoutAsync();
            }
        }
    }
}
