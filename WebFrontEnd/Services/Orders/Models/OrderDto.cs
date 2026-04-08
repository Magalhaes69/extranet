
using System.ComponentModel.DataAnnotations;

namespace WebFrontEnd.Services.Orders.Models
{
    public class OrderDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        public string CardCode { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime? DocDueDate { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string? CardName { get; set; }
        public string? Referencia { get; set; }
        public string? Address { get; set; }
        public string? MailAddress { get; set; }
        public string? MailZipCode { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Fax { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? MaxCommitment { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? PriceListNum { get; set; }
        public string? DocCurrency { get; set; }
        public string? Comments { get; set; }
        public string? NumAtCard { get; set; }
        public string? DocType { get; set; }
        public string? OwnerCode { get; set; }
        public int? Series { get; set; }
        public string? CntctCode { get; set; }
        public string? ShipToCode { get; set; }
        public string? Address2 { get; set; }
        public string? PayToCode { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? DocTotal { get; set; }
        public decimal? VatSum { get; set; }
        public List<OrderLineDto> DocumentLines { get; set; } = new();
    }
}
