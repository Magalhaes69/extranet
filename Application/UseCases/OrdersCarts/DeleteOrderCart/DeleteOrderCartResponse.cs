

namespace Application.UseCases.OrdersCarts.DeleteOrderCart
{
    public sealed record DeleteOrderCartResponse
    {
        public Guid OrderId { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string? Referencia { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
    }
}
