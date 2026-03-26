using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Invoices.GetAllInvoices
{
    public record GetAllInvoicesResponse
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string? DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? Address { get; set; }
        public decimal? DocTotal { get; set; }
        public string? DocCurrency { get; set; }
        public string? Reference1 { get; set; }
        public string? Reference2 { get; set; }
        public string? Comments { get; set; }
        public decimal? TotalDiscount { get; set; }
       

        public List<GetAllDocumentLinesResponse>? DocumentLines { get; set; }
    }
}
