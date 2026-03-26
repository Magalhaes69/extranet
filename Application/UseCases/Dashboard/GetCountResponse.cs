using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Dashboard
{
    public sealed record GetCountResponse
    {
        public int Total { get; set; }
    }
}
