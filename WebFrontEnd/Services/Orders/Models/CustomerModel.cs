namespace WebFrontEnd.Services.Orders.Models
{
    public class CustomerModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public AddressModel Address { get; set; } = new();
        public List<OrderModel> Orders { get; set; } = new();
    }
}
