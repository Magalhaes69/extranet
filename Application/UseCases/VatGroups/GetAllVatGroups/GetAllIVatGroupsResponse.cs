using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.VatGroups.GetAllVatGroups
{
    public record GetAllIVatGroupsResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string EU { get; set; }

        public List<GetAllVatGroupLinesResponse>? VatGroups_Lines { get; set; }
    }
}
