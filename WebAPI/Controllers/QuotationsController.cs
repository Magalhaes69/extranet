using Application.UseCases.Quotations.DetailsQuotations;
using Application.UseCases.Quotations.GetAllQuotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QuotationsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("list")]
        public async Task<IActionResult> GetAllQuotations([FromBody] GetAllQuotationsRequest request, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }
        [HttpGet("details")]
        public async Task<IActionResult> GetDetailsQuotations([FromQuery] string CardCode, int DocEntry, CancellationToken cancellationToken)
        {
            var request = new DetailsQuotationsRequest { CardCode = CardCode, DocEntry = DocEntry };

            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }
    }
}
