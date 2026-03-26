
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
            //if (!string.IsNullOrEmpty(request.CardType))
                //filter += $" and CardType eq '{request.CardType}";

            var getPartner= await _sapClient.GetAsync<GetAllBusinessPartnersResponse>("BusinessPartners", filter, cancellationToken);

            return new GetAllBusinessPartnersResponse
            {
                CardCode = getPartner!.CardCode!,
                CardName = getPartner.CardName,
                CardType = getPartner.CardType,
                GroupCode = getPartner.GroupCode,
                Address = getPartner.Address,
                MailAddress = getPartner.MailAddress,
                MailZipCode = getPartner.MailZipCode,
                Phone1 = getPartner.Phone1,
                Phone2 = getPartner.Phone2,
                Fax = getPartner.Fax,
                CreditLimit = getPartner.CreditLimit,
                MaxCommitment = getPartner.MaxCommitment,
                DiscountPercent = getPartner.DiscountPercent,
                PriceListNum = getPartner.PriceListNum,
                Currency = getPartner.Currency,
                City = getPartner.City,
                Country = getPartner.Country,
                County = getPartner.County,
                BilltoDefault = getPartner.BilltoDefault,
                BPAddresses = getPartner.BPAddresses!.Select(p => new GetAllBPAddressesResponse
                {
                    AddressName = p.AddressName,
                    Street = p.Street,
                    Block = p.Block,
                    ZipCode = p.ZipCode,
                    City = p.City,
                    County = p.County,
                    Country = p.Country,
                }).ToList(),
                ContactEmployees = getPartner.ContactEmployees!.Select(x=> new GetAllContactEmployeesResponse
                {
                    CardCode = x.CardCode,
                    Name = x.Name,
                    Position = x.Position,
                    Address = x.Address,
                    Phone1 = x.Phone1,
                    Phone2 = x.Phone2,
                    MobilePhone = x.MobilePhone,
                    Fax = x.Fax,
                    E_Mail = x.E_Mail,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                }).ToList(),
            };
        }
    }
}
