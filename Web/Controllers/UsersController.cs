using Common.Dtos;
using Common.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;
        private readonly AppSettings _appSettings;

        public UsersController(IMediator mediator, ILogger<UsersController> logger, IOptions<AppSettings> appSettings)
        {
            _mediator = mediator;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> AddUser([FromBody] CreateApplicationUserCommand request)
        {
            var result = await _mediator.Send(request);
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
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateApplicationUserProfileCommand request)
        {
            var result = await _mediator.Send(request);
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
            var result = await _mediator.Send(request);
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
            var result = await _mediator.Send(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("datatable/all")]
        public async Task<IActionResult> GetUsersDataTable(GetUsersDataTableRequest request)
        {
            var result = await _mediator.Send(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentUsers()
        {
            GetMostRecentUsersRequest request = new GetMostRecentUsersRequest() 
            { Count = _appSettings.Partners.MruListCount };

            var result = await _mediator.Send(request);
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
