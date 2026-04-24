using Application.UseCases.Items;
using Application.UseCases.Items.GetAllItemsCatalog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET
        [HttpGet("list")]
        public async Task<IActionResult> GetAllItems([FromQuery] string Valid, CancellationToken cancellationToken)
        {
            var request = new GetAllItemsRequest { Valid = Valid };
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("catalog")]
        public async Task<IActionResult> GetAllItemsCatalog([FromBody] GetAllItemsCatalogRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

    }
}
