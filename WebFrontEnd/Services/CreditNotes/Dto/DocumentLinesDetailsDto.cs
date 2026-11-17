namespace WebFrontEnd.Services.CreditNotes.Dto
{
    public sealed record DocumentLinesDetailsDto : DocumentLineBaseDto
    {
        public bool Selected { get; set; }
        public DateTime? ShipDate { get; set; }
        public string? SerialNum { get; set; }
    }
}
