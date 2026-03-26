using Application.UseCases.Dashboard.Orders;
using Application.UseCases.Orders.GetAllOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DashboardController(IMediator mediator)
        {
            
            _mediator = mediator;
        }
        [HttpGet("summary")]
        public async Task<IActionResult> GetAllOrder( CancellationToken cancellationToken)
        {
            var request = new GetCountOrdersRequest { CardCode = "CL10001", Status = string.Empty };

            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }
    }
}
