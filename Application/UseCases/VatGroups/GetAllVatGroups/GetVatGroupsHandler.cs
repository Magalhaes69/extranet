using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.VatGroups.GetAllVatGroups
{
    public class GetVatGroupsHandler : IRequestHandler<GetAllVatGroupsRequest, List<GetAllIVatGroupsResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetVatGroupsHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<List<GetAllIVatGroupsResponse>> Handle(GetAllVatGroupsRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.Inative))
                filter = $"Inactive eq '{request.Inative}' &$orderby=Code";

            var getVat = await _sapClient.GetAllAsync<GetAllIVatGroupsResponse>("VatGroups", filter, cancellationToken);

            return getVat.Select(x => new GetAllIVatGroupsResponse
            {
                Code = x.Code,
                Name = x.Name,
                Category = x.Category,
                EU = x.EU,
                VatGroups_Lines = x.VatGroups_Lines!.Select(v => new GetAllVatGroupLinesResponse
                {
                    Effectivefrom = v.Effectivefrom,
                    Rate = v.Rate,
                    EqualizationTax = v.EqualizationTax,
                }).ToList(),
            }).ToList();
        }
    }
}
