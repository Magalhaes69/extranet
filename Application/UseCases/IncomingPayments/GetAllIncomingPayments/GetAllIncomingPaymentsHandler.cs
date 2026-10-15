using Application.Shared.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public class GetAllIncomingPaymentsHandler : IRequestHandler<GetAllIncomingPaymentsRequest, GetAllIncomingPaymentsResult>
    {
        private readonly ISapClient _sapClient;
        public GetAllIncomingPaymentsHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<GetAllIncomingPaymentsResult> Handle(GetAllIncomingPaymentsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = ODataFilterBuilder.Build(request);
                var queryParams = new Dictionary<string, string>
                {
                    ["$select"] = "DocEntry," +
                    "DocNum," +
                    "DocType," +
                    "Series," +
                    "DocDate," +
                    "CardCode," +
                    "CardName," +
                    "Address," +
                    "CashSum," +
                    "DocCurrency," +
                    "Reference1," +
                    "Reference2," +
                    "Remarks," +
                    "CheckAccount," +
                    "TransferAccount," +
                    "TransferSum," +
                    "TransferDate," +
                    "LocalCurrency," +
                    "TaxDate," +
                    "ProjectCode," +
                    "DeductionPercent," +
                    "DeductionSum," +
                    "CashSumFC," +
                    "CashSumSys," +
                    "DueDate," +
                    "PaymentChecks," +
                    "Cancelled",
                    
                    ["$orderby"] = "DocEntry desc"
                };
                var (data, total) = await _sapClient.GetPagedBL1Async<GetAllIncomingPaymentsResponse>(
                    "IncomingPayments",
                    request.Page,
                    request.PageSize,
                    filter,
                    queryParams,
                    cancellationToken);

                // Get total Of Checks and Transfers
                //queryParams = new Dictionary<string, string>
                //{
                //    ["$select"] = "PaymentChecks",
                //    ["$orderby"] = "DocEntry desc"
                //};

                //var getResults = await _sapClient.GetAllB1Async<GetAllIncomingPaymentsPayChecksResponse>(
                //    "IncomingPayments",
                //    filter,
                //    queryParams,
                //    cancellationToken);


                var result = new GetAllIncomingPaymentsResult
                {
                    Items = data ?? new List<GetAllIncomingPaymentsResponse>(),
                    TotalCount = total,
                };

                if (request.Page == 1)
                {
                    result.Total = await _sapClient.GetTotalsAsync<GetAllIncomingPaymentsTotalResult>(
                        "IncomingPayments",
                        filter,
                        cancellationToken);


                    // 3. Check total (seeded SQL query)
                    if (IncomingPaymentCheckTotalParams.TryBuild(request, out var sqlParams))
                    {
                        var checks = await _sapClient.ExecuteQueryObjAsync<CheckTotalRowResponse>(
                            "IncomingPaymentsCheckTotal",
                            sqlParams.ToDictionary(x => x.Key, x => (object)x.Value),
                            cancellationToken);

                        result.Total.TotalPaymentChecks = checks?.TotalCheckSum ?? 0;
                    }
                    else
                    {
                        result.Total.TotalPaymentChecks = 0;
                    }

                    result.Total.Currency = await _sapClient.GetLocalCurrencyAsync(cancellationToken);
                }



                //result.Total.TotalPaymentChecks = getResults
                //    .SelectMany(x => x.PaymentChecks ?? Enumerable.Empty<GetAllPaymentChecksResponse>())
                //    .Sum(x => x.CheckSum);

                return result;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        
    }
}
