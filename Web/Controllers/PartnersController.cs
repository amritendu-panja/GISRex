using Common.Dtos;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class PartnersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public PartnersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("add")]
		public async Task<IActionResult> CreatePartner([FromBody] CreatePartnerCommand command)
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
	}
}
