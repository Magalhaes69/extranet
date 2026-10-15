namespace WebFrontEnd.Services.SQLQueries.Dto
{
    public sealed record ContactEmployeesResponse
    {
        public int CntctCode { get; set; }
        public string? CardCode { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Address { get; set; }
        public string? Tel1 { get; set; }
        public string? Tel2 { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
