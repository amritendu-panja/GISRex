using Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class GroupsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public GroupsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("all")]
		public async Task<IActionResult> GetAllGroups()
		{
			GetAllGroupsRequest request = new GetAllGroupsRequest();
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
		public async Task<IActionResult> GetGroupsDataTable(GetGroupsDataTableRequest request)
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

		[HttpGet("{id}")]
        public async Task<IActionResult> GetGroupByid(int id)
        {
            GetGroupByIdRequest request = new GetGroupByIdRequest();
			request.GroupId = id;
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
