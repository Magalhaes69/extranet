using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotations.GetAllQuotations
{
    public record GetAllQuotationsResponse
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string? DocType { get; set; }
        public string? HandWritten { get; set; }
        public string? Printed { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? Address { get; set; }
        public decimal? DocTotal { get; set; }
        public string? DocCurrency { get; set; }
        public decimal? DocRate { get; set; }
        public string? Reference1 { get; set; }
        public string? Reference2 { get; set; }
        public string? Comments { get; set; }
        public string? Confirmed { get; set; }
        public int? Series { get; set; }
        public DateTime? TaxDate { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? VatSum { get; set; }
        public decimal? VatSumSys { get; set; }
        public decimal? VatSumFc { get; set; }
        public decimal? DocTotalFc { get; set; }
        public decimal? DocTotalSys { get; set; }
        public string? DocumentStatus { get; set; }
        public string? PeriodIndicator { get; set; }
        public string? PayToCode { get; set; }
    }
}
