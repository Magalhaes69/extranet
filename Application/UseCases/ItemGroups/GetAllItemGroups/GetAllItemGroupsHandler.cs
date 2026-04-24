
using Application.UseCases.Invoices.Helps;
using Application.UseCases.Items;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.ItemGroups.GetAllItemGroups
{
    public class GetAllItemGroupsHandler : IRequestHandler<GetAllItemGroupsRequest, List<GetAllItemGroupsResponse>>
    {
        private readonly ISapClient _client;

        public GetAllItemGroupsHandler(ISapClient client)
        {
            _client = client;
        }
        public async Task<List<GetAllItemGroupsResponse>> Handle(GetAllItemGroupsRequest request, CancellationToken cancellationToken)
        {
            var filter = ODataFilterBuilder.Build(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "Number,GroupName",
                ["$orderby"] = "GroupName"
            };
            var getItems = await _client.GetAllB1Async<GetAllItemGroupsResponse>(
                "ItemGroups",
                filter,
                queryParams,
                cancellationToken);

            return getItems;
        }
    }
}
