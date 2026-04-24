using Application.UseCases.IncomingPayments.GetAllIncomingPayments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomingPaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public IncomingPaymentsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("list")]
        public async Task<IActionResult> GetAllOrder([FromBody] GetAllIncomingPaymentsRequest request, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }
    }
}
