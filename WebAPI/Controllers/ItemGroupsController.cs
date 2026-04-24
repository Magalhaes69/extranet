using Application.UseCases.ItemGroups.GetAllItemGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ItemGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetAllItems(CancellationToken cancellationToken)
        {
            var request = new GetAllItemGroupsRequest();
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
