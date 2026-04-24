namespace WebFrontEnd.Services.OrdersCarts.Dto
{
    public record UpdateOrdersCartDto
    {
        public Guid OrderId { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string? Referencia { get; set; }
        public bool Status { get; set; } = false;

        public string UserId { get; set; }
        public List<UpdateOrdersCartLineDto> OrderLineCarts { get; set; }
    }
}
