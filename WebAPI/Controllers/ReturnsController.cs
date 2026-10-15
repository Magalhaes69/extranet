using Application.UseCases.Returns.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReturnsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReturnsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("list")]
        public async Task<IActionResult> GetAllOrder([FromBody] GetAllReturnsRequest request, CancellationToken cancellationToken)
        {
            var cardCode = User.FindFirst("CardCode")?.Value; // same claim your GetCardCode() reads
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            request.DynamicFilters!.Add(new Domain.Helps.DynamicFilter
            {
                Value1 = cardCode,
                Field = "CardCode",
                Operator = Domain.Helps.Enum.OperatorType.Iqual
            });
            var orders = await _mediator.Send(request, cancellationToken);

            return Ok(orders);
        }

        //[HttpPost("details")]
        //public async Task<IActionResult> GetDetails([FromBody] GetAllReturnsRequest request, CancellationToken cancellationToken)
        //{
        //    var orders = await _mediator.Send(request, cancellationToken);
        //    return Ok(orders);
        //}
    }
}
