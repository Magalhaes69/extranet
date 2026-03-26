using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Orders
{
    public record SapOrderListDto
    {
        public List<SapOrderDto> Value { get; set; } = new();
    }
}
