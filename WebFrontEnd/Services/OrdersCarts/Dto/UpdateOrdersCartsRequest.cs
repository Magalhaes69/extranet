namespace WebFrontEnd.Services.OrdersCarts.Dto
{
    public record UpdateOrdersCartsRequest
    {
        public Guid OrderId { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
    }
}
