using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Orders;
using MediatR;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateB1OrderRequest : IRequest<CreateOrderResponse>
    {
        public string CardCode { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CardName { get; set; }
        public string Referencia { get; set; }
        public List<OrderLineDto> DocumentLines { get; set; } = new();
        

    }
}
