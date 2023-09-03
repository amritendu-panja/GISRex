using Common.Dtos;
using Common.Exceptions;
using Common.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthenticationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] CreateApplicationUserCommand request)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelException("Invalid request.");
            }

            var result = await mediator.Send(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelException("Invalid request.");
            }

            var result = await mediator.Send(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelException("Invalid request.");
            }

            var result = await mediator.Send(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            string? userId = HttpContext.User.FindFirstValue(Constants.JwtIdKey);
            if (userId == null)
            {
                throw new InvalidModelException("Invalid request.");
            }

            LogoutRequest request = new LogoutRequest { UserId = Convert.ToInt32(userId) };
            var result = await mediator.Send(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
