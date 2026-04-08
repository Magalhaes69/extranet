using Application.UseCases.DeliveryNotes.DetailsDeliveryNotes;
using Application.UseCases.DeliveryNotes.GetAllDeliverNotes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryNotesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeliveryNotesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("list")]
        public async Task<IActionResult> GetAllOrder([FromQuery] string CardCode, CancellationToken cancellationToken)
        {
            var request = new GetAllDeliveryNotesResquest { CardCode = CardCode };

            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetDetailsOrder([FromQuery] string CardCode, int DocEntry, CancellationToken cancellationToken)
        {
            var request = new DetailsDeliveryNotesRequest { CardCode = CardCode, DocEntry = DocEntry };
            var invoices = await _mediator.Send(request, cancellationToken);
            return Ok(invoices);
        }
    }
}
