using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders.CreateOrder
{
    public sealed record CreateOrderResponse
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string DocType { get; set; } = default!;
        public List<CreateOrderLineResponse> DocumentLines { get; set; } = new();
    }
}
