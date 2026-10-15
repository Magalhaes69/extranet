namespace WebFrontEnd.Services.Orders.Models
{
    public class OrderLineDto
    {
        private string _itemCode;
        public string ItemCode { 
            get => _itemCode; 
            set { 
                _itemCode = value;
                CalculateTotal();
            } 
        }

        private decimal _quantity;
        public decimal Quantity { get => _quantity; 
            set { 
                _quantity = value;
                CalculateTotal();

            } 
        }
        private decimal _unitPrice;
        public decimal UnitPrice { 
            get => _unitPrice; 
            set { 
                _unitPrice = value; 
                CalculateTotal(); 
            } 
        }
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
        private string _vatGroup;
       
        public string VatGroup {
            get => _vatGroup;
            set {
                _vatGroup = value;
                CalculateTotal();
            }
        }
        public decimal VatRate { get; set; } = 0m;
       
        public string? Currency { get; set; }
        private void CalculateTotal()
        {
            LineTotal = (Quantity * UnitPrice) ;
            VatAmount = (LineTotal * (VatRate / 100m));
        }
        public decimal VatAmount { get; set; }
    }
}
