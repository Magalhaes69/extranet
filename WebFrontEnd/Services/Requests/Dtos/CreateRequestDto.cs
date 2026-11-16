namespace WebFrontEnd.Services.Requests.Dtos
{
    public sealed class CreateRequestDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
