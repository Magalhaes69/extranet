using Application.Shared.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.DeliveryNotes.GetAllDeliverNotes
{
    public class GetAllDeliveryNotesHandler : IRequestHandler<GetAllDeliveryNotesResquest, List<DeliveryNotesResponse>>
    {
        private readonly ISapClient _sapClient;

        public GetAllDeliveryNotesHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<List<DeliveryNotesResponse>> Handle(GetAllDeliveryNotesResquest request, CancellationToken cancellationToken)
        {
            var filter = ODataFilterBuilder.Build(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "DocEntry,DocNum,DocType,Series,DocDate,DocDueDate,CardCode,CardName,Address,DocTotal,DocCurrency,Reference1,Reference2,Comments,TotalDiscount,DocumentStatus,Cancelled,DiscountPercent,VatSum,ShipToCode,PayToCode,Address2",
                ["$orderby"] = "DocEntry desc"
            };

            var sapOrders = await _sapClient.GetAllB1Async<DeliveryNotesResponse>(
                "DeliveryNotes",
                filter,
                queryParams,
                cancellationToken);

            return sapOrders;
        }
    }
}
