namespace WebFrontEnd.Services.BusinessPartners.Dto
{
    public sealed record BPAddressesDto
    {
        public string? AddressName { get; set; }
        public string? Street { get; set; }
        public string? Block { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? Country { get; set; }
    }
}
