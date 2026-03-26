using Application.UseCases.Items;
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
        public async Task<IActionResult> GetAllItems(CancellationToken cancellationToken)
        {
            var request = await _mediator.Send(new GetAllItemsRequest(), cancellationToken);
            return Ok(request);
        }

       
    }
}
