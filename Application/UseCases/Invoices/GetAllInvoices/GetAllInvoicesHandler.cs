using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Invoices.GetAllInvoices
{
    public class GetAllInvoicesHandler : IRequestHandler<GetAllInvoicesRequest, List<GetAllInvoicesResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetAllInvoicesHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }

        public async Task<List<GetAllInvoicesResponse>> Handle(GetAllInvoicesRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.CardCode))
                filter = $"CardCode eq '{request.CardCode}' &$orderby=DocEntry desc";

            var sapOrders = await _sapClient.GetAllAsync<GetAllInvoicesResponse>("Invoices", filter, cancellationToken);

            return sapOrders.Select(o => new GetAllInvoicesResponse
            {
                DocEntry = o.DocEntry,
                DocNum = o.DocNum,
                DocType = o.DocType,
                DocDate = o.DocDate,
                DocDueDate = o.DocDueDate,
                CardCode = o.CardCode,
                CardName = o.CardName,
                Address = o.Address,
                DocTotal = o.DocTotal,
                DocCurrency = o.DocCurrency,
                Comments = o.Comments,
                DocumentLines = o.DocumentLines!.Select(l => new GetAllDocumentLinesResponse
                {
                    LineNum = l.LineNum,
                    ItemCode = l.ItemCode,
                    ItemDescription = l.ItemDescription,
                    Quantity = l.Quantity,
                    Price = l.Price,
                    PriceAfterVAT = l.PriceAfterVAT,
                    Currency = l.Currency,
                    DiscountPercent = l.DiscountPercent,
                    SerialNum = l.SerialNum,
                    WarehouseCode = l.WarehouseCode,
                    UnitPrice = l.UnitPrice,
                    LineTotal = l.LineTotal,
                }).ToList()
            }).ToList();
        }
    }
}
