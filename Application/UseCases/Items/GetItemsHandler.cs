using Application.UseCases.Orders.GetAllOrders;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Items
{
    public class GetItemsHandler : IRequestHandler<GetAllItemsRequest, List<GetAllItemsResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetItemsHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }

        public async Task<List<GetAllItemsResponse>> Handle(GetAllItemsRequest request, CancellationToken cancellationToken)
        {
           var getItems = await _sapClient.GetAllAsync<GetAllItemsResponse>("Items", "", cancellationToken);

            return getItems.Select(x => new GetAllItemsResponse
            {
                ItemCode = x.ItemCode,
                ItemName = x.ItemName,
                ItemsGroupCode = x.ItemsGroupCode,
                ItemPrices = x.ItemPrices!.Select(p => new ItemPrices
                {
                    Price = p.Price,
                    Currency = p.Currency,
                    PriceList = p.PriceList,
                }).ToList()
            }).ToList();
        }
    }
}
