using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotations.GetAllQuotations
{
    public record GetAllDocumentLinesResponse
    {
        public int LineNum { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceAfterVAT { get; set; }
        public string? Currency { get; set; }
        public decimal? Rate { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? SerialNum { get; set; }
        public string? WarehouseCode { get; set; }
        public string? VatGroup { get; set; }
    }
}
