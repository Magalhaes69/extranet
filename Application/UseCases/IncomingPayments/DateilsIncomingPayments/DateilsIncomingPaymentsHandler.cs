using Application.UseCases.Invoices.GetAllInvoices;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.IncomingPayments.DateilsIncomingPayments
{
    public sealed class DateilsIncomingPaymentsHandler : IRequestHandler<DateilsIncomingPaymentsRequest, DateilsIncomingPaymentsResponse>
    {
        private readonly ISapClient _sapClient;
        public DateilsIncomingPaymentsHandler(ISapClient sapClient) => _sapClient = sapClient;

        public async Task<DateilsIncomingPaymentsResponse> Handle(DateilsIncomingPaymentsRequest request, CancellationToken cancellationToken)
        {
            try
            {

                string? filter = null;
                if (!string.IsNullOrEmpty(request.CardCode) && !string.IsNullOrEmpty(request.DocType))
                {
                    filter = $"CardCode eq '{request.CardCode}' and DocNum eq {request.DocNum} and DocType eq '{request.DocType}' and Series eq {request.Series}";
                }

                var response = await _sapClient.GetAsync<DateilsIncomingPaymentsResponse>(
                    "IncomingPayments", 
                    filter, 
                    cancellationToken);

                if(response!.PaymentInvoices is not null)
                {
                    var invoiceDocEntries = response?.PaymentInvoices?
                            .Select(x => x.DocEntry)
                            .Distinct()
                            .ToList();
                    var invoiceFilter =string.Join(" or ", invoiceDocEntries!.Select(de => $"DocEntry eq {de}"));
                    
                    var queryParams = new Dictionary<string, string>
                     {
                         ["$select"] = "DocEntry,DocDate,DocNum,DocDueDate,DocTotal,DocCurrency,Comments,PaidToDate",
                         ["$orderby"] = "DocEntry desc"
                     };

                    var sapInvoices = await _sapClient.GetAllB1Async<GetAllInvoicesResponse>(
                        "Invoices",
                        invoiceFilter,
                        queryParams,
                        cancellationToken);
                  
                    foreach(var item in response!.PaymentInvoices!)
                    {
                        var invoice = sapInvoices?.FirstOrDefault(x => x.DocEntry == item.DocEntry);
                        if(invoice is not null)
                        {
                            item.DocDate = invoice.DocDate;
                            item.DocDueDate = invoice.DocDueDate;
                            item.DocTotal = invoice.DocTotal;
                            item.DocCurrency = invoice.DocCurrency;
                            item.Comments = invoice.Comments;
                            item.DocNum = invoice.DocNum;
                            item.PaidToDate = invoice.PaidToDate;
                        }
                    }
                }

               
              
                return response!;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
