
using MediatR;

namespace Application.UseCases.DeliveryNotes.DetailsDeliveryNotes
{
    public class DetailsDeliveryNotesRequest : IRequest<DetailsDeliveryNotesResponse>
    {
        public string CardCode { get; set; }
        public int DocEntry { get; set; }
    }
}
