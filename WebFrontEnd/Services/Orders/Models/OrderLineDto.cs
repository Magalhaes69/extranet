namespace WebFrontEnd.Services.Orders.Models
{
    public class OrderLineDto
    {
        public string ItemCode { get; set; }
        private decimal _quantity;
        public decimal Quantity { get => _quantity; 
            set { 
                _quantity = value;
                CalculateTotal();

            } 
        }
        public decimal UnitPrice { get; set; }
        public string? _UnitPrice { 
            get => UnitPrice.ToString("N2", new System.Globalization.CultureInfo("pt-PT")) + " " + Currency; 
            set
            {
                if(decimal.TryParse(value, out var result))
                {
                    UnitPrice = result;
                }
            }
        }
        public string WarehouseCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal LineTotal { get; set; }
        public string _LineTotal { 
            get => LineTotal.ToString("N2", new System.Globalization.CultureInfo("pt-PT")) + " " + Currency;
            set
            {
                if (decimal.TryParse(value, out var result))
                {
                    UnitPrice = result;
                }
            }
        }
        public int Line { get; set; }
        public bool Selected { get; set; }
        public string VatGroup { get; set; }

        public string? Currency { get; set; }
        private void CalculateTotal()
        {
            LineTotal = Quantity * UnitPrice;
        }
    }
}
