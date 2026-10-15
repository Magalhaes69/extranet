

using Application.Shared.Helps;
using Application.UseCases.IncomingPayments.GetAllIncomingPayments;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Returns.GetAll
{
    public sealed class GetAllReturnsHandler : IRequestHandler<GetAllReturnsRequest, GetAllReturnsResult>
    {
        private readonly ISapClient _sapClient;
        public GetAllReturnsHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }

        public async Task<GetAllReturnsResult> Handle(GetAllReturnsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = ODataFilterBuilder.Build(request);
                var queryParams = new Dictionary<string, string>
                {
                    ["$select"] =""+
                             "DocEntry,"+
                             "DocNum,"+
                             "DocType,"+
                             "Series,"+
                             "DocDate,"+
                             "DocDueDate,"+
                             "TaxDate,"+
                             "CardCode,"+
                             "CardName,"+
                             "Address,"+
                             "NumAtCard,"+
                             "DocTotal,"+
                             "VatSum,"+
                             "DocTotalFc,"+
                             "DocCurrency,"+
                             "DocRate,"+
                             "Reference1,"+
                             "Reference2,"+
                             "Comments,"+
                             "JournalMemo,"+
                             "Project,"+
                             "ContactPersonCode,"+
                             "SalesPersonCode,"+
                             "PriceListNum,"+
                             "DocumentStatus,"+
                             "Cancelled,"+
                             "CancelStatus",

                    ["$orderby"] = "DocEntry desc"
                };
                var (data, total) = await _sapClient.GetPagedBL1Async<GetAllReturnsResponse>(
                    "Returns",
                    request.Page,
                    request.PageSize,
                    filter,
                    queryParams,
                    cancellationToken);

                var result = new GetAllReturnsResult
                {
                    Items = data ?? new List<GetAllReturnsResponse>(),
                    TotalCount = total,
                };
              
                return result;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
