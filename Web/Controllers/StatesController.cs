using Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllStates()
        {
            GetAllStatesRequest request = new GetAllStatesRequest();
            var states = await _mediator.Send(request);
            return Ok(states);
        }

        [HttpGet]
        [Route("{countryCode}")]
        public async Task<IActionResult> GetStatesByCountry(string countryCode)
        {
            GetStatesByCountryRequest request = new GetStatesByCountryRequest() { CountryCode = countryCode };
            var states = await _mediator.Send(request);
            return Ok(states);
        }
    }
}
