namespace WebFrontEnd.Services.Orders.Models
{
    public class OrderModel
    {
        public string OrderId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
