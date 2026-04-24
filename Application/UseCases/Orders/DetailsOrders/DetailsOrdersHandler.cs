using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Orders.DetailsOrders
{
    public class DetailsOrdersHandler : IRequestHandler<DetailsOrdersRequest, DetailsOrdersResponse>
    {
        private readonly ISapClient _sapClient;

        public DetailsOrdersHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<DetailsOrdersResponse> Handle(DetailsOrdersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string? filter = null;
                if (!string.IsNullOrEmpty(request.CardCode))
                {
                    filter = $"CardCode eq '{request.CardCode}' and DocEntry eq {request.DocEntry}";
                }
                else
                {
                    filter = $"DocEntry eq {request.DocEntry}";
                }

                var response = await _sapClient.GetAsync<DetailsOrdersResponse>("Orders",filter, cancellationToken);

                return response!;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
