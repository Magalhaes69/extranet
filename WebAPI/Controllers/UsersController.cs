using Application.Dtos;
//using Application.UseCases.onexuser.Create;
using Application.UseCases.OrdersCarts.CreateOrderCart;
using Application.UseCases.User.Auth;
using Application.UseCases.User.CreateUser;
using Application.UseCases.User.DefinePassword;
using Application.UseCases.User.GetAllUser;
using Application.UseCases.User.GetUserById;
using Application.UseCases.User.UpdateUser;
using Application.UseCases.User.UserProfile;
using Application.UseCases.User.Verify2FA;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(IMediator mediator, UserManager<AppUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
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

        // [HttpPost("verify-2fa")]
        // public async Task<IActionResult> Verify2FA(Verify2FARequest wl, CancellationToken cancellationToken)
        // {
        //     var result = await _mediator.Send(request);
        //     return Ok(result);
        // }

        /// <summary>
        /// Actualiza o perfil do PRÓPRIO utilizador autenticado (Definições). O utilizador vem sempre do token:
        /// o cliente não escolhe o Id, nem consegue alterar email, NUIT ou CardCode.
        /// </summary>
        [HttpPut("updateprofile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateProfile(
            [FromForm] UpdateProfileForm form,
            IFormFile? file,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var user = string.IsNullOrWhiteSpace(userId) ? null : await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound(new ServiceResponse { Flag = false, Message = "Utilizador não encontrado." });

            var request = new UpdateUserRequest
            {
                Id = user.Id,
                Email = user.Email,
                Nuit = user.Nuit,
                CardCode = user.CardCode,
                ImageUrl = user.ImageUrl,
                FirstName = form.FirstName,
                LastName = form.LastName,
                Theme = form.Theme,
                Language = form.Language,
                PhoneNumber = form.PhoneNumber,
                TwoFactorEnabled = form.TwoFactorEnabled
            };

            if (file is { Length: > 0 })
            {
                if (file.Length > 5 * 1024 * 1024 || !file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new ServiceResponse { Flag = false, Message = "A imagem deve ter no máximo 5 MB." });

                request.FileName = file.FileName;
                request.ImageStream = file.OpenReadStream();
            }

            try
            {
                await _mediator.Send(request, cancellationToken);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ServiceResponse
                {
                    Flag = false,
                    Message = string.Join(" ", ex.Errors.Select(e => e.ErrorMessage))
                });
            }

            return Ok(new ServiceResponse { Flag = true, Message = "Perfil actualizado com sucesso." });
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

    /// <summary>Campos editáveis do perfil enviados pelas Definições (multipart/form-data).</summary>
    public class UpdateProfileForm
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Theme { get; set; } = "light";
        public string? Language { get; set; }
        public string? PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
