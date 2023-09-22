using Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private ILogger<CountriesController> logger;
        private readonly IMediator _mediator;

        public CountriesController(ILogger<CountriesController> logger, IMediator mediator)
        {
            this.logger = logger;
            this._mediator = mediator;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllCountries()
        {
            GetAllCountriesRequest request = new GetAllCountriesRequest();
            var countries = await _mediator.Send(request);
            return Ok(countries);
        }

        [HttpGet]
        [Route("{countryCode}/callingcode")]
        public async Task<IActionResult> GetCallingCodeByCountry([FromRoute] string countryCode)
        {
            GetCallingCodeByCountryRequest request = new GetCallingCodeByCountryRequest() { CountryCode = countryCode };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
