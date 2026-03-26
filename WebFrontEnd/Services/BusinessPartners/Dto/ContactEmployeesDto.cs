namespace WebFrontEnd.Services.BusinessPartners.Dto
{
    public record ContactEmployeesDto
    {
        public string? CardCode { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Address { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? MobilePhone { get; set; }
        public string? Fax { get; set; }
        public string? E_Mail { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
    }
}
