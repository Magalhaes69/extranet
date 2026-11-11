namespace WebFrontEnd.Services.Requests.Dtos
{
    public sealed class RequestListItemDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
