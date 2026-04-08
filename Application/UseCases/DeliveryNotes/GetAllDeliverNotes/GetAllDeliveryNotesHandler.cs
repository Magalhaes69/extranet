using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.DeliveryNotes.GetAllDeliverNotes
{
    public class GetAllDeliveryNotesHandler : IRequestHandler<GetAllDeliveryNotesResquest, List<DeliveryNotesResponse>>
    {
        private readonly ISapClient _sapClient;

        public GetAllDeliveryNotesHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<List<DeliveryNotesResponse>> Handle(GetAllDeliveryNotesResquest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.CardCode))
                filter = $"CardCode eq '{request.CardCode}' &$orderby=DocEntry desc";

            var sapOrders = await _sapClient.GetAllAsync<DeliveryNotesResponse>("DeliveryNotes", filter, cancellationToken);

            return sapOrders;
        }
    }
}
