using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.DeliveryNotes.DetailsDeliveryNotes
{
    public class DetailsDeliveryNotesHandler : IRequestHandler<DetailsDeliveryNotesRequest, DetailsDeliveryNotesResponse>
    {
        private readonly ISapClient _sapClient;
        public DetailsDeliveryNotesHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<DetailsDeliveryNotesResponse> Handle(DetailsDeliveryNotesRequest request, CancellationToken cancellationToken)
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

                var response = await _sapClient.GetAsync<DetailsDeliveryNotesResponse>("DeliveryNotes", filter, cancellationToken);
                return response!;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
