using Common.Dtos;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            if(!ModelState.IsValid)
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

    }
}
