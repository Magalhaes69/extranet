using WebFrontEnd.Helps;
using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.VatGroups.DTO
{
    public record VatGroupsDto
    {
        [Filterable(LabelKey = "code")]
        public string Code { get; set; }
        [Filterable(LabelKey = "name")]
        public string Name { get; set; }
        public string Category { get; set; }
        public string? EU { get; set; }
        public string? EUDisplay => YesNoMapper.ToDisplay(EU!);

        public List<VatGroupLinesDto>? VatGroups_Lines { get; set; }
    }
}
