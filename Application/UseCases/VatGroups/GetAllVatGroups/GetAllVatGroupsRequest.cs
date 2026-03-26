using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.UseCases.VatGroups.GetAllVatGroups
{
    public class GetAllVatGroupsRequest : IRequest<List<GetAllIVatGroupsResponse>>
    {
        public string? Inative { get; set; }
    }
}
