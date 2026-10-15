using Application.Dtos;
using Application.Shared.Behavior;
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
        public async Task<IActionResult> CreateOrder([FromBody] CreateB1OrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(request, cancellationToken);

                return Ok(new ServiceResponse
                {
                    Flag = true,
                    Message = "Order created successfully",
                    DocEntry = result.DocEntry
                });
            }
            catch (SapB1Exception ex)
            {
                // SAP business error, e.g. "Item is inactive [RDR1.ItemCode][line: 1]"
                return BadRequest(new ServiceResponse
                {
                    Flag = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Unexpected error creating SAP order");

                return StatusCode(500, new ServiceResponse
                {
                    Flag = false,
                    Message = "Unexpected error while creating the order."
                });
            }
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAllOrder ([FromBody] GetAllOrdersRequest ordersRequest, CancellationToken cancellationToken)
        { 
            var orders = await _mediator.Send(ordersRequest, cancellationToken);
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
