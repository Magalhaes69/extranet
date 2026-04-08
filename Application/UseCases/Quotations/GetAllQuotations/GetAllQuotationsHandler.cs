using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Quotations.GetAllQuotations
{
    public class GetAllQuotationsHandler : IRequestHandler<GetAllQuotationsRequest, List<GetAllQuotationsResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetAllQuotationsHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<List<GetAllQuotationsResponse>> Handle(GetAllQuotationsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string? filter = null;
                if (!string.IsNullOrEmpty(request.CardCode))
                    filter = $"CardCode eq '{request.CardCode}' &$orderby=DocEntry desc";

                var sapOrders = await _sapClient.GetAllAsync<GetAllQuotationsResponse>("Quotations", filter, cancellationToken);

                return sapOrders.ToList();
            }
            catch(Exception ex)
            {
                return null!;
            }
        }
    }
}
