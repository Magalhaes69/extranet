using Application.UseCases.Quotations.GetAllQuotations;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Quotations.DetailsQuotations
{
    public class DetailsQuotationsHandler : IRequestHandler<DetailsQuotationsRequest, DetailsQuotationsResponse>
    {
        private readonly ISapClient _sapClient;
        public DetailsQuotationsHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<DetailsQuotationsResponse> Handle(DetailsQuotationsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string? filter = null;
                if (!string.IsNullOrEmpty(request.CardCode))
                    filter = $"CardCode eq '{request.CardCode}' and DocEntry eq {request.DocEntry}";


                filter += $"&$orderby=DocEntry desc";


                var sapOrders = await _sapClient.GetAsync<DetailsQuotationsResponse>("Quotations", filter, cancellationToken);

                return sapOrders!;
            }
            catch (Exception ex)
            {
                return new DetailsQuotationsResponse();
            }
        }
    }
}
