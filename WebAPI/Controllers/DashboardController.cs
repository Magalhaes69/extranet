using Application.Shared.Behavior;
using Application.UseCases.Dashboard;
using Application.UseCases.Dashboard.Orders;
using Application.UseCases.Orders.GetAllOrders;
using Domain;
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

        [HttpGet("metrics/{key}")]
        public async Task<IActionResult> GetMetric(string key, CancellationToken ct)
        {
            var cardCode = User.FindFirst("CardCode")?.Value; // same claim your GetCardCode() reads
            if (string.IsNullOrWhiteSpace(cardCode))
                return BadRequest(new ServiceResponse
                {
                    Flag = false,
                    Message = "CardCode claim not found in token."
                });

            if (!DashboardMetrics.All.ContainsKey(key))
                return NotFound(new ServiceResponse
                {
                    Flag = false,
                    Message = $"Unknown metric '{key}'."
                });

            try
            {
                return Ok(await _mediator.Send(new GetDashboardMetricRequest(key, cardCode), ct));
            }
            catch (SapB1Exception ex)
            {
                return BadRequest(new ServiceResponse { Flag = false, Message = ex.Message });
            }
        }
    }
}
