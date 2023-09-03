using Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LayersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LayersController> _logger;

        public LayersController(IMediator mediator, ILogger<LayersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetLayer(Guid id)
        {
            GetApplicationLayerRequest request = new GetApplicationLayerRequest { Id = id };
            var result = await _mediator.Send(request);            
            return Ok(result);            
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateLayer([FromBody] CreateAplicationLayerCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
