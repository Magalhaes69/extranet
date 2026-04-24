using Application.UseCases.Orders.DetailsOrders;
using MediatR;

namespace Application.UseCases.Invoices.DetailsInvoices
{
    public class DetailsInvoicesRequest : IRequest<DetailsInvoicesResponse>
    {
        public string CardCode { get; set; }
        public int DocEntry { get; set; }
    }
}
