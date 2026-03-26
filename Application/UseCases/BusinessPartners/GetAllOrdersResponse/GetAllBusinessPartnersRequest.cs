using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.UseCases.BusinessPartners.GetAllOrdersResponse
{
    public class GetAllBusinessPartnersRequest : IRequest<GetAllBusinessPartnersResponse>
    {
        public string CardCode { get; set; }
        public string? CardType { get; set; } = string.Empty;
    }
}
