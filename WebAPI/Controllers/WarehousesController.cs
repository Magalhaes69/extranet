using System.Globalization;
using Application.UseCases.Items;
using Application.UseCases.Warehouses.GetAllWarehouses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WarehousesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllWarehouses([FromQuery] string Inactive, CancellationToken cancellationToken)
        {
            var get = new GetAllWarehousesRequest { Inactive  = Inactive };
            var request = await _mediator.Send(get, cancellationToken);
            return Ok(request);
        }
    }
}
