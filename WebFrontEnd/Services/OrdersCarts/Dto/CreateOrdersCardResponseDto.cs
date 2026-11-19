namespace WebFrontEnd.Services.OrdersCarts.Dto
{
    public class CreateOrdersCardResponseDto
    {
        public Guid OrderId { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Referencia { get; set; }
    }
}
