using Application.UseCases.Invoices.GetAllInvoices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InvoicesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("list")]
        public async Task<IActionResult> GetAllOrder([FromQuery] string CardCode, CancellationToken cancellationToken)
        {
            var request = new GetAllInvoicesRequest { CardCode = CardCode };

            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }
    }
}
