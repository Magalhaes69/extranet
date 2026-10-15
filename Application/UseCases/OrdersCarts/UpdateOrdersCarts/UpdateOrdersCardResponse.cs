namespace Application.UseCases.OrdersCarts.UpdateOrdersCarts
{
    public sealed record UpdateOrdersCardResponse
    {
        public Guid OrderId { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string? Referencia { get; set; }
        public bool Status { get; set; }
    }
}
