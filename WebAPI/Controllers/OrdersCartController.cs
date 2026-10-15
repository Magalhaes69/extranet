using Application.UseCases.OrdersCarts.CreateOrderCart;
using Application.UseCases.OrdersCarts.CreateOrderCartLine;
using Application.UseCases.OrdersCarts.DeleteOrderCart;
using Application.UseCases.OrdersCarts.DeleteOrderCartLine;
using Application.UseCases.OrdersCarts.GetLastOrderCartPend;
using Application.UseCases.OrdersCarts.UpdateOrderCartLine;
using Application.UseCases.OrdersCarts.UpdateOrdersCarts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersCartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersCartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceGeneralResponse<CreateOrdersCardResponse>>> CreateOrdersCart([FromBody] CreateOrdersCardRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result == null)
            {
                return BadRequest(new ServiceGeneralResponse<object>
                {
                    Flag = false,
                    Message = "Badrequest",
                    Data = null!
                });
            }

            return Ok(new ServiceGeneralResponse<CreateOrdersCardResponse>
            {
                Flag = true,
                Message = "Actualizado com sucesso.",
                Data = result
            });
        }

        [HttpGet("pendent")]
        public async Task<ActionResult<GetLastOrdersCartPendResponse>> GetOrdersCartLast([FromQuery] string CardCode, string UserId, CancellationToken cancellationToken)
        {
            var request = new GetLastOrdersCartPendRequest()
            {
                CardCode = CardCode,
                UserId = UserId
            };
            var result = await _mediator.Send(request);

            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult<ServiceGeneralResponse<UpdateOrderCartLineResponse>>> UpdateOrdersCartLine([FromBody] UpdateOrderCartLineRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request);

            if (result == null)
            {
                return BadRequest(new ServiceGeneralResponse<object>
                {
                    Flag = false,
                    Message = "Badrequest",
                    Data = null!
                });
            }

            return Ok(new ServiceGeneralResponse<UpdateOrderCartLineResponse>
            {
                Flag = true,
                Message = "Actualizado com sucesso.",
                Data = result
            });
        }

        [HttpPut("update-orders")]
        public async Task<ActionResult<ServiceGeneralResponse<UpdateOrderCartLineResponse>>> UpdateOrdersCart([FromBody] UpdateOrdersCartsRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request);

            if (result == null)
            {
                return BadRequest(new ServiceGeneralResponse<object>
                {
                    Flag = false,
                    Message = "Badrequest",
                    Data = null!
                });
            }

            return Ok(new ServiceGeneralResponse<UpdateOrdersCardResponse>
            {
                Flag = true,
                Message = "Actualizado com sucesso.",
                Data = result
            });
        }

        [HttpPost("addnew")]
        public async Task<ActionResult<ServiceGeneralResponse<CreateOrderCartLineResponse>>> CreateOrdersCartLine([FromBody] CreateOrderCartLineRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request);

            if(result == null)
            {
                return BadRequest(new ServiceGeneralResponse<object>
                {
                    Flag = false,
                    Message = "Badrequest",
                    Data = null!
                });
            }

            return Ok(new ServiceGeneralResponse<CreateOrderCartLineResponse>
            {
                Flag = true,
                Message = "Adicionado com sucesso.",
                Data = result
            });
           
        }

        [HttpDelete("deleteline")]
        public async Task<ActionResult<ServiceGeneralResponse<DeleteOrderCartLineResponse>>> DeleteOrdersCartLine([FromQuery] string OrderLineCartId, string OrderId, CancellationToken cancellationToken)
        {

            var request = new DeleteOrderCartLineRequest()
            {
                OrderLineCartId = Guid.Parse(OrderLineCartId),
                OrderId = Guid.Parse(OrderId),
            };

            var result = await _mediator.Send(request);

            if (result == null)
            {
                return BadRequest(new ServiceGeneralResponse<object>
                {
                    Flag = false,
                    Message = "Badrequest",
                    Data = null!
                });
            }

            return Ok(new ServiceGeneralResponse<DeleteOrderCartLineResponse>
            {
                Flag = true,
                Message = "Item removed.",
                Data = null,
                Datalist = result
            });

        }

        [HttpDelete("delete-order")]
        public async Task<ActionResult<ServiceGeneralResponse<DeleteOrderCartResponse>>> DeleteByOrderId([FromQuery] string OrderId, CancellationToken cancellationToken)
        {

            var request = new DeleteOrderCartRequest()
            { 
                OrderId = Guid.Parse(OrderId),
            };

            var result = await _mediator.Send(request);

            if (result == null)
            {
                return BadRequest(new ServiceGeneralResponse<object>
                {
                    Flag = false,
                    Message = "Badrequest",
                    Data = null!
                });
            }

            return Ok(new ServiceGeneralResponse<DeleteOrderCartResponse>
            {
                Flag = true,
                Message = "Order deleted successful.",
                Data = result,
                Datalist = null!
            });

        }
    }
}
