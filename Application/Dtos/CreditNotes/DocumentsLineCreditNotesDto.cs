
namespace Application.Dtos.CreditNotes
{
    public record DocumentsLineCreditNotesDto : DocumentLineDto
    {
        public DateTime? ShipDate { get; set; }
        public string? VendorNum { get; set; }
        public string? SerialNum { get; set; }
    }
}
