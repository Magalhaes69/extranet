namespace WebFrontEnd.Shared.Dtos
{
    public record TotalResultDto
    {
        public decimal Total { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}
