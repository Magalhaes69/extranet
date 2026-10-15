using Application.Shared.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.VatGroups.GetAllVatGroups
{
    public class GetVatGroupsHandler : IRequestHandler<GetAllVatGroupsRequest, GetAllIVatGroupsResult>
    {
        private readonly ISapClient _sapClient;
        public GetVatGroupsHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<GetAllIVatGroupsResult> Handle(GetAllVatGroupsRequest request, CancellationToken cancellationToken)
        {

            var filter = ODataFilterBuilder.Build(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "Code,Name,Category,EU,VatGroups_Lines"
            };


            var (getVat, total) = await _sapClient.GetPagedBL1Async<GetAllIVatGroupsResponse>(
                "VatGroups",
                request.Page,
                request.PageSize,
                filter, 
                queryParams,
                cancellationToken);

            return new GetAllIVatGroupsResult
            {
                Items = getVat,
                TotalCount = total
            };
        }
    }
}
