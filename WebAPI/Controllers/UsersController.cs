using Application.Dtos;
//using Application.UseCases.onexuser.Create;
using Application.UseCases.OrdersCarts.CreateOrderCart;
using Application.UseCases.User.Auth;
using Application.UseCases.User.CreateUser;
using Application.UseCases.User.DefinePassword;
using Application.UseCases.User.GetAllUser;
using Application.UseCases.User.GetUserById;
using Application.UseCases.User.UserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest createUser)
        {
            var response = await _mediator.Send(createUser);
            return Ok(response);
        }
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var response = await _mediator.Send(loginRequest);
            return Ok(response);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile([FromQuery] string email, CancellationToken cancellation)
        {

            var request = new GetProfileRequest { Email = email };
            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("getAll")]
        [Authorize]
        public async Task<ActionResult<List<UserDto>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetAllUserRequest(), cancellationToken);
            return Ok(response);
        }
        [HttpGet("getbyid")]
        [Authorize]
        public async Task<ActionResult<List<GetUserByIdResponse>>> GetById([FromQuery] string Id, CancellationToken cancellationToken)
        {
            var request = new GetUserByIdRequest() { Id = Id };

            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result == null)
            {
                return BadRequest(new ServiceGeneralResponse<object>
                {
                    Flag = false,
                    Message = "Badrequest",
                    Data = null!
                });
            }
            return Ok(new ServiceGeneralResponse<SetPasswordResponse>
            {
                Flag = true,
                Message = "Actualizado com sucesso.",
                Data = result
            });
        }
     
    }
}
