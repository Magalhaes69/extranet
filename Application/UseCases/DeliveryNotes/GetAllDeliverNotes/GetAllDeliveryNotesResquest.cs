using MediatR;

namespace Application.UseCases.DeliveryNotes.GetAllDeliverNotes
{
    public class GetAllDeliveryNotesResquest : IRequest<List<DeliveryNotesResponse>>
    {
        public string CardCode { get; set; }
    }
}
