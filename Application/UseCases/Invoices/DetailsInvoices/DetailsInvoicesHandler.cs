using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Invoices.DetailsInvoices
{
    public class DetailsInvoicesHandler : IRequestHandler<DetailsInvoicesRequest, DetailsInvoicesResponse>
    {
        private readonly ISapClient _sapClient;
        public DetailsInvoicesHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<DetailsInvoicesResponse> Handle(DetailsInvoicesRequest request, CancellationToken cancellationToken)
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

                var response = await _sapClient.GetAsync<DetailsInvoicesResponse>("Invoices", filter, cancellationToken);
                return response!;
            }
            catch (Exception ex)
            {
               return null!;
            }
        }
    }
}
