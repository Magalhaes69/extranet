using Application.UseCases.SQLQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SQLQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SQLQueriesController(IMediator mediator) => _mediator = mediator;

        [HttpPost("series")]
        public async Task<IActionResult> GetSeries([FromBody] SapQueryUIRequest requestUI, CancellationToken cancellationToken)
        {
            var request = new SapQueryRequest
            {
                Query = requestUI.SqlCode,
                ObjectCode = requestUI.Value
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);

        }
    }
}
