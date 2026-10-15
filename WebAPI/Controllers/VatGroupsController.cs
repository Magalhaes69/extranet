using Application.UseCases.Orders.GetAllOrders;
using Application.UseCases.VatGroups.GetAllVatGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VatGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VatGroupsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("list")]
        public async Task<IActionResult> GetAllVatGroup([FromBody] GetAllVatGroupsRequest request, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }
    }
}
