using Application.UseCases.BusinessPartners.GetAllOrdersResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BusinessPartnersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BusinessPartnersController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllBusinessPArtners([FromQuery] string CardCode, string? CardType, CancellationToken cancellationToken)
        {
            var request = new GetAllBusinessPartnersRequest
            {
                CardCode = CardCode,
                CardType = CardType,
            };

            var businessPartners = await _mediator.Send(request, cancellationToken);

            return Ok(businessPartners);
        }
    }
}
