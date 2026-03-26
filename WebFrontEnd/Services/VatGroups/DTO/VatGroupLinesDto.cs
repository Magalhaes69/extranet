namespace WebFrontEnd.Services.VatGroups.DTO
{
    public record VatGroupLinesDto
    {
        public string? Effectivefrom { get; set; }
        public decimal? Rate { get; set; }
        public decimal? EqualizationTax { get; set; }
    }
}
