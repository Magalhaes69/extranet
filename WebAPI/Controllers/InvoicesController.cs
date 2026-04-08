using Application.UseCases.Invoices.DetailsInvoices;
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

        [HttpPost("list")]
        public async Task<IActionResult> GetAllOrder([FromBody] GetAllInvoicesRequest request, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(request, cancellationToken);
            return Ok(orders);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetDetailsOrder([FromQuery] string CardCode, int DocEntry, CancellationToken cancellationToken)
        {
            var request = new DetailsInvoicesRequest { CardCode = CardCode, DocEntry = DocEntry };
            var invoices = await _mediator.Send(request, cancellationToken);
            return Ok(invoices);
        }
    }
}
