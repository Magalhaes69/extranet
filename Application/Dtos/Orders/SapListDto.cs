using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.Orders
{
    public class SapListDto <T>
    {
        public List<T> Value { get; set; } = new();
    }
}
