using Application.UseCases.IncomingPayments.DateilsIncomingPayments;
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
            var cardCode = User.FindFirst("CardCode")?.Value; // same claim your GetCardCode() reads
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            request.DynamicFilters!.Add(new Domain.Helps.DynamicFilter
            {
                Value1 = cardCode,
                Field = "CardCode",
                Operator = Domain.Helps.Enum.OperatorType.Iqual
            });

            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }

        [HttpPost("details")]
        public async Task<IActionResult> GetDetails([FromBody] DateilsIncomingPaymentsRequest request, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(request, cancellationToken);
            return Ok(orders);
        }
    }
}
