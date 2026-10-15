
using Application.Dtos.OrdersCards;
using Application.Shared.Helps;
using Application.UseCases.VatGroups.GetAllVatGroups;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Items.GetAllItemsCatalog
{
    public class GetItemsCatalogHandler : IRequestHandler<GetAllItemsCatalogRequest, List<CatalogItemDto>>
    {
        private readonly ISapClient _sapClient;
        private readonly IItemExtensionRepository _repository;
        public GetItemsCatalogHandler(ISapClient sapClient, IItemExtensionRepository repository)
        {
            this._sapClient = sapClient;
            _repository = repository;
        }
        public async Task<List<CatalogItemDto>> Handle(GetAllItemsCatalogRequest request, CancellationToken cancellationToken)
        {
            var filter = ODataFilterBuilder.BuildCatalog(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "ItemCode,ItemName,ItemsGroupCode,ItemPrices,SalesVATGroup,DefaultWarehouse",
                ["$orderby"] = "ItemCode"
            };
            var sapItems = await _sapClient.GetPagedB1Async<GetAllItemsResponse>(
                "Items",
                request.Page,
                request.PageSize,
                filter,
                queryParams,
                cancellationToken);

            var itemCodes = sapItems.Select(x => x.ItemCode).ToList();

            var extensions = await _repository.GetAllByListItemCodeAsync(itemCodes);
            var extensionDict = extensions.ToDictionary(x => x.ItemCode, x => x.ImageUrl);


            var vatFilter = string.Join(" or ", 
                sapItems
                .Select(x=> x.SalesVATGroup)
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => $"Code eq '{x}'")
                .Distinct()
                );

            var queryParamsVat = new Dictionary<string, string>
            {
                ["$select"] = "Code,VatGroups_Lines"
            };

            var queryVatGroups = await _sapClient.GetAllB1Async<GetAllIVatGroupsResponse>(
                "VatGroups",
                vatFilter,
                queryParamsVat,
                cancellationToken);

            var vatDictionary = queryVatGroups?
                .GroupBy(x => x.Code)
                .ToDictionary(g => g.Key, g => g.First()
                );
            var result = sapItems.Select(item =>
            {
                extensionDict.TryGetValue(item.ItemCode, out var image);
                if (vatDictionary != null &&
                item.SalesVATGroup != null &&
                vatDictionary.TryGetValue(item.SalesVATGroup, out var vatGroup)) ;


                return new CatalogItemDto
                {
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    ItemsGroupCode = item.ItemsGroupCode,
                    ItemPrices = item.ItemPrices?.Where(x => x.PriceList == request.TPriceBusiness).FirstOrDefault()?.Price ?? 0,
                    Currency = item.ItemPrices?.Where(x => x.PriceList == request.TPriceBusiness).FirstOrDefault()?.Currency ?? string.Empty,
                    ImageUrl = image ?? "/images/items/default.png",
                    DefaultWarehouse = item.DefaultWarehouse ?? string.Empty,
                    SalesVATGroup = item.SalesVATGroup ?? string.Empty,
                    Rate = item.Rate ?? 0,
                };
            }).ToList();

            


            return result;
        }
    }
}
