using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders.CreateOrder
{
    public sealed record CreateOrderLineResponse
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; } = default!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
