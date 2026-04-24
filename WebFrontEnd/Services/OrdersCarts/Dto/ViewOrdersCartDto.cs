namespace WebFrontEnd.Services.OrdersCarts.Dto
{
    public class ViewOrdersCartDto
    {
        public Guid OrderId { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string? Referencia { get; set; }
        public bool Status { get; set; } = false;

        public string UserId { get; set; }
        public List<ViewOrderLineCartDto>? OrderLineCarts { get; set; }
    }
}
