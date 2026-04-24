namespace Application.UseCases.OrdersCarts.CreateOrderCartLine
{
    public sealed record CreateOrderCartLineResponse
    {
        public Guid OrderLineCartId { get; set; }
    }
}
