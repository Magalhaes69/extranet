using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders.GetAllOrders
{
    public sealed record GetAllOrdersResponse
    {
        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public string? DocType { get; set; }

        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }

        public string? CardCode { get; set; }
        public string? CardName { get; set; }

        public string? Address { get; set; }

        public decimal? DocTotal { get; set; }
        public decimal? VatSum { get; set; }

        public string? DocCurrency { get; set; }
        public double? DocRate { get; set; }

        public string? Comments { get; set; }
        public string? JournalMemo { get; set; }

        public int? SalesPersonCode { get; set; }

        public int? Series { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int? UserSign { get; set; }

        public string? DocumentStatus { get; set; }
        public string? Cancelled { get; set; }

        public string? Confirmed { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal? TotalDiscount { get; set; }

        public string? ControlAccount { get; set; }

        public int? DataVersion { get; set; }
        public List<GetAllOrdersLineResponse> Lines { get; set; } = new();
    }
}
