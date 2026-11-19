namespace WebFrontEnd.Services.OrdersCarts.Dto
{
    public record CreateOrderLineCartExistOrderDto : OrderLineCartBaseDto
    {
        public Guid OrderId { get; set; }
    }
}
