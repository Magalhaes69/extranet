using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.UseCases.Invoices.GetAllInvoices
{
    public class GetAllInvoicesRequest : IRequest<List<GetAllInvoicesResponse>>
    {
        public string CardCode { get; set; }
    }
}
