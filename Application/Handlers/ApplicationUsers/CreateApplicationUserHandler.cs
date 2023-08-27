using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Mappings;
using MediatR;

namespace Application.Handlers.ApplicationUsers
{
    public class CreateApplicationUserHandler : IRequestHandler<CreateApplicationUserCommand, ApplicationUserResponseDto>
    {
        //private readonly ILogger<CreateApplicationUserHandler> _logger;
        private readonly IRepository<ApplicationUser> _repository;
        private readonly SharedMapping _mapping;

        public CreateApplicationUserHandler(IRepository<ApplicationUser> repository, SharedMapping mapping)
        {
            _repository = repository;
            _mapping = mapping;
        }

        public async Task<ApplicationUserResponseDto> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ApplicationUser applicationUser = new ApplicationUser(request.UserName, request.PasswordSalt, request.Email);
                var newUser = await _repository.AddAsync(applicationUser);
                var response = new ApplicationUserResponseDto();
                _mapping.Map(newUser, response);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
