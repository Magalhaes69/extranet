using Application.UseCases.Attachments2.GetAttachment;
using Application.UseCases.BusinessPartners.GetAllOrdersResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class Attachments2Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public Attachments2Controller(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("getattachment")]
        public async Task<IActionResult> GetAllBusinessPArtners([FromQuery] int AbsoluteEntry, CancellationToken cancellationToken)
        {
            var request = new GetAttachments2Request
            {
                AbsoluteEntry = AbsoluteEntry
            };

            var file = await _mediator.Send(request, cancellationToken);

            return File(file.Stream, file.ContentType); // ✅ ONLY THIS
        }
    }
}
