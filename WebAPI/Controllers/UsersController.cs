using System.Security.Claims;
using Application.Dtos;
using Application.UseCases.User.Auth;
using Application.UseCases.User.CreateUser;
using Application.UseCases.User.GetAllUser;
using Application.UseCases.User.UserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            var request = new UserDto { Id = Guid.Parse(userId) };
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

    }
}
