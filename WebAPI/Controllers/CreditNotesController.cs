using Application.UseCases.CreditNotes.DetailsCreditNotes;
using Application.UseCases.CreditNotes.GetAllCreditNotes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreditNotesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CreditNotesController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAllCreditNotes([FromBody] GetAllCreditNotesRequest request, CancellationToken cancellationToken)
        {
            var credit = await _mediator.Send(request, cancellationToken);
            return Ok(credit);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetCreditNotes([FromQuery] string CardCode, int DocEntry, CancellationToken cancellationToken)
        {
            var request = new DetailsCreditNotesRequest { CardCode = CardCode, DocEntry = DocEntry };
            var credit = await _mediator.Send(request, cancellationToken);

            return Ok(credit);
        }
    }
}
