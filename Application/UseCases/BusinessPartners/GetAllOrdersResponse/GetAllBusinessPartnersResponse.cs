namespace Application.UseCases.BusinessPartners.GetAllOrdersResponse
{
    public record GetAllBusinessPartnersResponse
    {
        public string CardCode { get; set; }
        public string? CardName { get; set; }
        public string? CardType { get; set; }
        public int? GroupCode { get; set; }
        public string? Address { get; set; }
        public string? MailAddress { get; set; }
        public string? MailZipCode { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Fax { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? MaxCommitment { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? PriceListNum { get; set; }
        public string? Currency { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? Country { get; set; }
        public string? BilltoDefault { get; set; }

        public List<GetAllBPAddressesResponse>? BPAddresses { get; set; }
        public List<GetAllContactEmployeesResponse>? ContactEmployees { get; set; }
    }
}
