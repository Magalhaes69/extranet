namespace Application.UseCases.OrdersCarts.CreateOrderCart
{
    public sealed record CreateOrdersCardResponse
    {
        public Guid OrderId { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Referencia { get; set; }
    }
}
