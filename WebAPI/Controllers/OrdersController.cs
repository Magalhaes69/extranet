using Application.Dtos;
using Application.UseCases.Orders.CreateOrder;
using Application.UseCases.Orders.DetailsOrders;
using Application.UseCases.Orders.GetAllOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateB1OrderRequest request)
        {
            var result = await _mediator.Send(request);

            var response = new ServiceResponse
            {
                Flag = true,
                Message = "Order created successfully",
                DocEntry = result.DocEntry
            };
            return Ok(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllOrder ([FromQuery] string CardCode, CancellationToken cancellationToken)
        { 
            var request = new GetAllOrdersRequest { CardCode = CardCode };

            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetdetailsOrder([FromQuery] string CardCode, int DocEntry, CancellationToken cancellationToken)
        {
            var request = new DetailsOrdersRequest { CardCode = CardCode, DocEntry = DocEntry };

            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }
    }
}
