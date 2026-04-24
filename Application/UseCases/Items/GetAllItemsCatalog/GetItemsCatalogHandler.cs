
using Application.Dtos.OrdersCards;
using Application.UseCases.Invoices.Helps;
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
                ["$select"] = "ItemCode,ItemName,ItemsGroupCode,ItemPrices",
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

            var result = sapItems.Select(item =>
            {
                extensionDict.TryGetValue(item.ItemCode, out var image);

                return new CatalogItemDto
                {
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    ItemsGroupCode = item.ItemsGroupCode,
                    ItemPrices = item.ItemPrices?.Where(x => x.PriceList == request.TPriceBusiness).FirstOrDefault()?.Price ?? 0,
                    Currency = item.ItemPrices?.Where(x => x.PriceList == request.TPriceBusiness).FirstOrDefault()?.Currency ?? string.Empty,
                    ImageUrl = image ?? "/images/items/default.png"
                };
            }).ToList();

            return result;
        }
    }
}
