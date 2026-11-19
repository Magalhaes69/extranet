namespace WebFrontEnd.Services.Orders.Models
{
    public class OrdersCreate
    {
        public string CardCode { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CardName { get; set; }
        public string Referencia { get; set; }
        public List<DocumentLines>? DocumentLines { get; set; }
    }
}
