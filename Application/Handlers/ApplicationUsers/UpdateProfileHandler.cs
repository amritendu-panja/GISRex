using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class UpdateProfileHandler : IRequestHandler<UpdateApplicationUserProfileCommand, GetApplicationUserResponseDto>
    {
        private readonly IRepository<ApplicationUserDetails> _repository;
        private readonly SharedMapping _mapping;
        private readonly ILogger<UpdateProfileHandler> _logger;
        private readonly IMediator _mediator;

        public UpdateProfileHandler(
            IRepository<ApplicationUserDetails> repository, 
            SharedMapping mapping, 
            IMediator mediator,
            ILogger<UpdateProfileHandler> logger)
        {
            _repository = repository;
            _mapping = mapping;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<GetApplicationUserResponseDto> Handle(UpdateApplicationUserProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saving profile details for {0}", request.UserName);
            try
            {
                var currentRecord = _repository.Find(c => c.UserId == request.UserId).First();
                _mapping.Map(request, currentRecord);
                await _repository.UpdateAsync(currentRecord);

                FindByUsernameRequest findByUsernameRequest = new FindByUsernameRequest { Username = request.UserName };
                return await _mediator.Send(findByUsernameRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving profile for {0}", request.UserName);                
                throw new DbException(ex.Message);
            }
        }
    }
}
