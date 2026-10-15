namespace WebFrontEnd.Services.IncomingPayments.Dto
{
    public record GetByDocNumIncomingPaymentsRequest
    {
        public string CardCode { get; set; }
        public int DocNum { get; set; }
        public string DocType { get; set; }
        public int Series { get; set; }
    }
}
