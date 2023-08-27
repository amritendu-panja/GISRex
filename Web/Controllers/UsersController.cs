using Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
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
    }
}
