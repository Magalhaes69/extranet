using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Items
{
    public sealed record GetAllItemsResponse
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ForeignName { get; set; }
        public int ItemsGroupCode { get; set; }
        public List<ItemPrices>? ItemPrices { get; set; }
    }
}
