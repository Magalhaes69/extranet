using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.VatGroups.GetAllVatGroups
{
    public record GetAllVatGroupLinesResponse
    {
        public string? Effectivefrom { get; set; }
        public decimal? Rate { get; set; }
        public decimal? EqualizationTax { get; set; }
    }
}
