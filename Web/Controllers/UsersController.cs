using Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<UsersController> logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> AddUser([FromBody] CreateApplicationUserCommand request)
        {
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
        [Route("profile/update")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileCommand request)
        {
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            GetApplicationUserRequest request = new GetApplicationUserRequest { UserGuid = id };
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

        [AllowAnonymous]
        [HttpGet]
        [Route("findname/{userName}")]
        public async Task<IActionResult> FindByUserName(string userName)
        {
            FindByUsernameRequest request = new FindByUsernameRequest { Username = userName };
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
