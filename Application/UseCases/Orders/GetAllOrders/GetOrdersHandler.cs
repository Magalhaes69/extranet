using Application.Dtos.Orders;
using Application.UseCases.Orders.CreateOrder;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Orders.GetAllOrders
{
    public class GetOrdersHandler : IRequestHandler<GetAllOrdersRequest, List<GetAllOrdersResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetOrdersHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }

        public async Task<List<GetAllOrdersResponse>> Handle(GetAllOrdersRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.CardCode))
                filter = $"CardCode eq '{request.CardCode}' &$orderby=DocEntry desc";

            var sapOrders = await _sapClient.GetAllAsync<SapOrderDto>("Orders", filter, cancellationToken);

            return sapOrders.Select(o => new GetAllOrdersResponse
            {
                DocEntry = o.DocEntry,
                DocNum = o.DocNum,
                DocType = o.DocType,
                DocDate = o.DocDate,
                DocDueDate = o.DocDueDate,
                CardCode = o.CardCode,
                CardName = o.CardName,
                Address = o.Address,
                DocTotal = o.DocTotal,
                VatSum = o.VatSum,
                DocCurrency = o.DocCurrency,
                DocRate = o.DocRate,
                Comments = o.Comments,
                JournalMemo = o.JournalMemo,
                SalesPersonCode = o.SalesPersonCode,
                Series = o.Series,
                CreationDate = o.CreationDate,
                UpdateDate = o.UpdateDate,
                UserSign = o.UserSign,
                DocumentStatus = o.DocumentStatus,
                Cancelled = o.Cancelled,
                Confirmed = o.Confirmed,
                DiscountPercent = o.DiscountPercent,
                TotalDiscount = o.TotalDiscount,
                ControlAccount = o.ControlAccount,
                DataVersion = o.DataVersion,
                Lines = o.DocumentLines.Select(l => new GetAllOrdersLineResponse
                {
                    LineNum = l.LineNum,
                    ItemCode = l.ItemCode,
                    ItemDescription = l.ItemDescription,
                    Quantity = l.Quantity,
                    ShipDate = l.ShipDate,
                    Price = l.Price,
                    PriceAfterVAT = l.PriceAfterVAT,
                    Currency = l.Currency,
                    Rate = l.Rate,
                    DiscountPercent = l.DiscountPercent,
                    VendorNum = l.VendorNum,
                    SerialNum = l.SerialNum,
                    WarehouseCode = l.WarehouseCode,
                    SalesPersonCode = l.SalesPersonCode,
                    CommisionPercent =  l.CommisionPercent,
                    TreeType = l.TreeType,
                    AccountCode = l.AccountCode,
                    UseBaseUnits = l.UseBaseUnits,
                    SupplierCatNum = l.SupplierCatNum,
                    CostingCode = l.CostingCode,
                    ProjectCode = l.ProjectCode,
                    BarCode = l.BarCode,
                    VatGroup = l.VatGroup,
                    Height1 = l.Height1,
                    Height2 = l.Height2,
                    Lengh1 = l.Lengh1,
                    Lengh2 = l.Lengh2,
                    Weight1 = l.Weight1,
                    Weight2 = l.Weight2,
                    Factor1 = l.Factor1,
                    Factor2 = l.Factor2,
                    Factor3 = l.Factor3,
                    Factor4 = l.Factor4,
                    BaseType = l.BaseType,
                    BaseEntry = l.BaseEntry,
                    BaseLine = l.BaseLine,
                    Volume = l.Volume,
                    Width1 = l.Width1,
                    Width2 = l.Width2,
                    Address = l.Address,
                    TaxCode = l.TaxCode,
                    TaxType = l.TaxType,
                    TaxLiable = l.TaxLiable,
                    PickStatus = l.PickStatus,
                    PickQuantity = l.PickQuantity,
                    PickListIdNumber = l.PickListIdNumber,
                    OriginalItem = l.OriginalItem,
                    BackOrder = l.BackOrder,
                    FreeText = l.FreeText,
                    ShippingMethod = l.ShippingMethod,
                    POTargetNum = l.POTargetNum,
                    POTargetRowNum = l.POTargetRowNum,
                    POTargetEntry = l.POTargetEntry,    
                    AppliedTax = l.AppliedTax,
                    AppliedTaxFC = l.AppliedTaxFC,
                    AppliedTaxSC = l.AppliedTaxSC,
                    WTLiable = l.WTLiable,
                    DeferredTax = l.DeferredTax,
                    EqualizationTaxPercent = l.EqualizationTaxPercent,
                    TotalEqualizationTax = l.TotalEqualizationTax,
                    TotalEqualizationTaxFC = l.TotalEqualizationTaxFC,
                    TotalEqualizationTaxSC = l.TotalEqualizationTaxSC,
                    NetTaxAmount = l.NetTaxAmount,
                    NetTaxAmountFC = l.NetTaxAmountFC,
                    NetTaxAmountSC = l.NetTaxAmountSC,
                    MeasureUnit = l.MeasureUnit,
                    UnitsOfMeasurment = l.UnitsOfMeasurment,
                    LineStatus = l.LineStatus,
                    LineTotal = l.LineTotal,
                    TaxPercentagePerRow = l.TaxPercentagePerRow,
                    TaxTotal = l.TaxTotal,
                    ExciseAmount = l.ExciseAmount,
                    TaxPerUnit = l.TaxPerUnit,
                    TotalInclTax = l.TotalInclTax,
                    RowTotalFC = l.RowTotalFC,
                    RowTotalSC = l.RowTotalSC,
                    LineType = l.LineType,
                    PackageQuantity = l.PackageQuantity,
                    COGSAccountCode = l.COGSAccountCode,
                    GrossBuyPrice = l.GrossBuyPrice,
                    GrossProfit = l.GrossProfit,
                    GrossProfitFC = l.GrossProfitFC,
                    GrossProfitSC = l.GrossProfitSC,
                    UoMEntry = l.UoMEntry,
                    UoMCode = l.UoMCode,
                    InventoryQuantity = l.InventoryQuantity,
                    RemainingOpenInventoryQuantity = l.RemainingOpenInventoryQuantity,
                    DocEntry = l.DocEntry,
                    OpenAmount = l.OpenAmount,
                    OpenAmountFC = l.OpenAmountFC,
                    OpenAmountSC = l.OpenAmountSC,
                    RemainingOpenQuantity = l.RemainingOpenQuantity,
                    UnitPrice = l.UnitPrice
                }).ToList()
            }).ToList();
        }
    }
}
