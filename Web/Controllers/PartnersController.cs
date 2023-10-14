using Common.Dtos;
using Common.Exceptions;
using Common.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Web.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class PartnersController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly AppSettings _appSettings;

		public PartnersController(IMediator mediator, IOptions<AppSettings> appSettings)
		{
			_mediator = mediator;
			_appSettings = appSettings.Value;
		}

		[HttpPost("add")]
		public async Task<IActionResult> CreatePartner([FromBody] CreateApplicationOrganizationCommand command)
		{
			if (!ModelState.IsValid)
			{
				throw new InvalidModelException("Invalid request.");
			}

			var result = await _mediator.Send(command);
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
        public async Task<IActionResult> GetRecentPartners()
		{
			GetMostRecentPartnersRequest request = new GetMostRecentPartnersRequest() { Count = _appSettings.Partners.MruListCount };
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

		[HttpGet("{Id:int}")]
		public async Task<IActionResult> GetPartnerById(int Id)
		{
			GetApplicationOrganizationByIdRequest request = new GetApplicationOrganizationByIdRequest()
			{
				OrganizationId = Id
			};

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
