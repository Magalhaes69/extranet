using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Invoices.GetAllInvoices;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public class GetAllIncomingPaymentsHandler : IRequestHandler<GetAllIncomingPaymentsRequest, List<GetAllIncomingPaymentsResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetAllIncomingPaymentsHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<List<GetAllIncomingPaymentsResponse>> Handle(GetAllIncomingPaymentsRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.CardCode))
                filter = $"CardCode eq '{request.CardCode}' &$orderby=DocEntry desc";

            var sapOrders = await _sapClient.GetAllAsync<GetAllIncomingPaymentsResponse>("IncomingPayments", filter, cancellationToken);
            
            return sapOrders.ToList();
        }
    }
}
