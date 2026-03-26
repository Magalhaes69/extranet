using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders.GetAllOrders
{
    public sealed record GetAllOrdersLineResponse
    {
        public int LineNum { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }

        public decimal? Quantity { get; set; }
        public DateTime? ShipDate { get; set; }

        public decimal? Price { get; set; }
        public decimal? PriceAfterVAT { get; set; }

        public string? Currency { get; set; }
        public decimal? Rate { get; set; }

        public decimal? DiscountPercent { get; set; }

        public string? VendorNum { get; set; }
        public string? SerialNum { get; set; }

        public string? WarehouseCode { get; set; }

        public int? SalesPersonCode { get; set; }

        public decimal? CommisionPercent { get; set; }

        public string? TreeType { get; set; }

        public string? AccountCode { get; set; }

        public string? UseBaseUnits { get; set; }

        public string? SupplierCatNum { get; set; }

        public string? CostingCode { get; set; }

        public string? ProjectCode { get; set; }

        public string? BarCode { get; set; }

        public string? VatGroup { get; set; }

        public decimal? Height1 { get; set; }
        public decimal? Height2 { get; set; }

        public decimal? Lengh1 { get; set; }
        public decimal? Lengh2 { get; set; }

        public decimal? Weight1 { get; set; }
        public decimal? Weight2 { get; set; }

        public decimal? Factor1 { get; set; }
        public decimal?   Factor2 { get; set; }
        public decimal? Factor3 { get; set; }
        public decimal? Factor4 { get; set; }

        public int? BaseType { get; set; }
        public int? BaseEntry { get; set; }
        public int? BaseLine { get; set; }

        public decimal? Volume { get; set; }
        

        public decimal? Width1 { get; set; }
        public decimal? Width2 { get; set; }

        public string? Address { get; set; }

        public string? TaxCode { get; set; }
        public string? TaxType { get; set; }

        public string? TaxLiable { get; set; }

        public string? PickStatus { get; set; }

        public decimal? PickQuantity { get; set; }

        public int? PickListIdNumber { get; set; }

        public string? OriginalItem { get; set; }

        public string? BackOrder { get; set; }

        public string? FreeText { get; set; }

        public int? ShippingMethod { get; set; }

        public int? POTargetNum { get; set; }

        public string? POTargetEntry { get; set; }

        public int? POTargetRowNum { get; set; }

        public decimal? AppliedTax { get; set; }
        public decimal? AppliedTaxFC { get; set; }
        public decimal? AppliedTaxSC { get; set; }

        public string? WTLiable { get; set; }

        public string? DeferredTax { get; set; }

        public decimal? EqualizationTaxPercent { get; set; }

        public decimal? TotalEqualizationTax { get; set; }
        public decimal? TotalEqualizationTaxFC { get; set; }
        public decimal? TotalEqualizationTaxSC { get; set; }

        public decimal? NetTaxAmount { get; set; }
        public decimal? NetTaxAmountFC { get; set; }
        public decimal? NetTaxAmountSC { get; set; }

        public string? MeasureUnit { get; set; }

        public decimal? UnitsOfMeasurment { get; set; }

        public decimal? LineTotal { get; set; }

        public decimal? TaxPercentagePerRow { get; set; }

        public decimal? TaxTotal { get; set; }

        public decimal? ExciseAmount { get; set; }

        public decimal? TaxPerUnit { get; set; }

        public decimal? TotalInclTax { get; set; }

        public decimal? RowTotalFC { get; set; }

        public decimal? RowTotalSC { get; set; }

        public decimal? UnitPrice { get; set; }

        public string? LineStatus { get; set; }

        public decimal? PackageQuantity { get; set; }

        public string? LineType { get; set; }

        public string? COGSAccountCode { get; set; }

        public decimal? GrossBuyPrice { get; set; }

        public decimal? GrossProfit { get; set; }

        public decimal? GrossProfitFC { get; set; }

        public decimal? GrossProfitSC { get; set; }

        public int? UoMEntry { get; set; }

        public string? UoMCode { get; set; }

        public decimal? InventoryQuantity { get; set; }

        public decimal? RemainingOpenInventoryQuantity { get; set; }

        public int? DocEntry { get; set; }

        public decimal? OpenAmount { get; set; }

        public decimal? OpenAmountFC { get; set; }

        public decimal? OpenAmountSC { get; set; }

        public decimal? RemainingOpenQuantity { get; set; }
    }
}
