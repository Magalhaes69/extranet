
namespace WebFrontEnd.Services.OrdersCarts.Dto
{
    public record CreateOrdersCartDto
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string? Referencia { get; set; }
        public string UserId { get; set; }
        public List<CreateOrderLineCartDto> OrderLineCart { get; set; }
    }
}
