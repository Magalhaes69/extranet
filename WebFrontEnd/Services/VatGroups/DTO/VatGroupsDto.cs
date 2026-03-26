using WebFrontEnd.Helps;

namespace WebFrontEnd.Services.VatGroups.DTO
{
    public record VatGroupsDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string? EU { get; set; }
        public string? EUDisplay => YesNoMapper.ToDisplay(EU!);

        public List<VatGroupLinesDto>? VatGroups_Lines { get; set; }
    }
}
