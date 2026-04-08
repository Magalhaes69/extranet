using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.CreditNotes.DetailsCreditNotes
{
    public class DetailsCreditNotesHandler : IRequestHandler<DetailsCreditNotesRequest, DetailsCreditNotesResponse>
    {
        private readonly ISapClient _client;

        public DetailsCreditNotesHandler(ISapClient client)
        {
            _client = client;
        }
        public async Task<DetailsCreditNotesResponse> Handle(DetailsCreditNotesRequest request, CancellationToken cancellationToken)
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

                var response = await _client.GetAsync<DetailsCreditNotesResponse>("CreditNotes", filter, cancellationToken);
                return response!;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
