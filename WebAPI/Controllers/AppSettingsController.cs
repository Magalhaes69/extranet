using Application.UseCases.AppSettings.CreateAppSetting;
using Application.UseCases.AppSettings.GetAllAppSetting;
using Application.UseCases.ItemExtensions.GetAllItemExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppSettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppSettingsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateAppSettingRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result == null)
            {
                return NotFound(new ServiceGeneralResponse<CreateAppSettingResponse>
                {
                    Flag = false,
                    Message = "Item extension not found",
                    Data = null
                });
            }

            return Ok(new ServiceGeneralResponse<CreateAppSettingResponse>
            {
                Flag = true,
                Message = "Item extension created successfully.",
                Data = result
            });
        }

        // GET
        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var request = new GetAllAppSettingRequest();
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

    }
}
