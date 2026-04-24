
using Application.UseCases.Items;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.BusinessPartners.GetAllOrdersResponse
{
    public class GetAllBusinessPartnersHandler : IRequestHandler<GetAllBusinessPartnersRequest, GetAllBusinessPartnersResponse>
    {
        private readonly ISapClient _sapClient;
        public GetAllBusinessPartnersHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }

        public async Task<GetAllBusinessPartnersResponse> Handle(GetAllBusinessPartnersRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.CardCode))
                filter = $"CardCode eq '{request.CardCode}'";
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "CardCode,CardName,CardType,GroupCode,Address,MailAddress,MailZipCode,Phone1,Phone2,Fax,CreditLimit,MaxCommitment,DiscountPercent,PriceListNum,Currency,City,County,Country,BilltoDefault,BPAddresses,ContactEmployees",
                ["$orderby"] = "CardCode"
            };

            return await _sapClient.GetB1Async<GetAllBusinessPartnersResponse>(
                "BusinessPartners", 
                filter, 
                queryParams,
                cancellationToken);

            
        }
    }
}
