﻿using Common.Dtos;
using Common.Exceptions;
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

        [HttpGet("org/{orgGuid}")]
        public async Task<IActionResult> GetPartnerByGuid(string orgGuid)
        {
            GetOrganizationByGuidRequest request = new GetOrganizationByGuidRequest()
            {
                OrgGuid = Guid.Parse(orgGuid)
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

        [HttpGet("checkdomain/{domain}")]
		public async Task<IActionResult> CheckDomainExists(string domain)
		{
			CheckDomainExistsRequest request = new CheckDomainExistsRequest()
			{
				Domain = domain
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

        [HttpPost("datatable/all")]
        public async Task<IActionResult> GetUsersDataTable(GetOrganizationsDataTableRequest request)
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

		[HttpPost("user/create")]
		public async Task<IActionResult> CreateUser(CreateOrganizationUserCommand request)
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

		[HttpGet("user/{userGuid}")]
        public async Task<IActionResult> GetUser(string userGuid)
        {
			GetOrganizationUserRequest request = new GetOrganizationUserRequest() { UserGuid = Guid.Parse(userGuid) };
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

		[HttpPost("user/update")]
        public async Task<IActionResult> UpdateUser(UpdateOrganizationUserProfileCommand command)
        {
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

        [HttpGet("{organizationId:int}/users/recent")]
        public async Task<IActionResult> GetRecentUsers(int organizationId, [FromQuery(Name = "count")] int count)
        {
            GetMostRecentOrganizationUsersRequest request = new GetMostRecentOrganizationUsersRequest()
            {
                OrganizationId = organizationId,
                Count = count
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
