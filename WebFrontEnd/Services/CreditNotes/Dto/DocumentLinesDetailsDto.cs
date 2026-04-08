namespace WebFrontEnd.Services.CreditNotes.Dto
{
    public sealed record DocumentLinesDetailsDto : DocumentLineBaseDto
    {
        public DateTime? ShipDate { get; set; }
        public decimal? DiscountPercent { get; set; }
      
        public string? SerialNum { get; set; }
    }
}
