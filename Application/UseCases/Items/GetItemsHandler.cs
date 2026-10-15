using Application.Shared.Helps;
using Application.UseCases.ItemGroups.GetAllItemGroups;
using Application.UseCases.VatGroups.GetAllVatGroups;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Items
{
    public class GetItemsHandler : IRequestHandler<GetAllItemsRequest, GetAllItemsResult>
    {
        private readonly ISapClient _sapClient;
        public GetItemsHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }

        public async Task<GetAllItemsResult> Handle(GetAllItemsRequest request, CancellationToken cancellationToken)
        {

            var filter = ODataFilterBuilder.Build(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "ItemCode,ItemName,ItemsGroupCode,ItemPrices,SalesVATGroup,DefaultWarehouse",
                ["$orderby"] = "ItemCode"
            };
            var (getItems, total) = await _sapClient.GetPagedBL1Async<GetAllItemsResponse>(
                "Items",
                request.Page,
                request.PageSize,
                filter,
                queryParams, 
                cancellationToken);

            var vatFilter = string.Join(" or ", 
                getItems
                .Select(x => x.SalesVATGroup)
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
                .ToDictionary(g => g.Key, g => g.First());


            var filterGroups = string.Join(" or ",
               getItems
               .Select(x => x.ItemsGroupCode)
               .Where(x => x != 0)
               .Select(x => $"Number eq {x}")
               .Distinct()
               );
            var queryParamsGroups = new Dictionary<string, string>
            {
                ["$select"] = "Number,GroupName,ItemGroupsWarehouseInfos",
                ["$orderby"] = "GroupName"
            };

            var queryGroups = await _sapClient.GetAllB1Async<GetAllItemGroupsResponse>(
                "ItemGroups",
                filterGroups,
                queryParamsGroups,
                cancellationToken);

            var groupDictionary = queryGroups?
                .GroupBy(x => x.Number)
                .ToDictionary(g => g.Key, g => g.First());

            if (getItems != null)
            {
                foreach (var item in getItems)
                {
                    // VAT
                    if (vatDictionary != null &&
                        item.SalesVATGroup != null &&
                        vatDictionary.TryGetValue(item.SalesVATGroup, out var vatGroup))
                    {
                        var vatLine = vatGroup.VatGroups_Lines?
                            .OrderByDescending(x => x.Effectivefrom)
                            .FirstOrDefault();

                        if (vatLine != null)
                        {
                            item.Rate = vatLine.Rate;
                        }
                    }

                    // Item Group
                    if (groupDictionary != null &&
                        groupDictionary.TryGetValue(item.ItemsGroupCode, out var group))
                    {
                        item.ItemsGroupName = group.GroupName;
                        var groupWarehouseInfo = group.ItemGroupsWarehouseInfos?
                            .OrderByDescending(x => x.WarehouseCode)
                            .FirstOrDefault();
                        if (groupWarehouseInfo != null)
                        {
                            if(string.IsNullOrEmpty(item.DefaultWarehouse))
                                item.DefaultWarehouse = groupWarehouseInfo.WarehouseCode;
                        }
                    }
                }
            }

            var result = new GetAllItemsResult
            {
                Items = getItems ?? new List<GetAllItemsResponse>(),
                TotalCount = total
            };

            return result;
        }
    }
}
