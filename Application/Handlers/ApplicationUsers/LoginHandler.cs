using Application.Helpers;
using Application.Repository;
using Common.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponseDto>
    {
        private readonly ILogger<LoginHandler> _logger;
        private readonly IApplicationUserRepository _repository;        
        private readonly IHashHelper _hashHelper;
        private readonly ITokenHelper _tokenHelper;

        public LoginHandler(ILogger<LoginHandler> logger, IApplicationUserRepository repository, IHashHelper hashHelper, ITokenHelper tokenHelper)
        {
            _logger = logger;
            _repository = repository;
            _hashHelper = hashHelper;
            _tokenHelper = tokenHelper;
        }

        public Task<LoginResponseDto> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var errorMessage = "Invalid username / password";
            LoginResponseDto response = new LoginResponseDto();
            var user = _repository.Find(u => u.UserName == request.Username).FirstOrDefault();
            if (user == null)
            {
                response.SetError(errorMessage);
                _logger.LogError(errorMessage);
            }
            else
            {
                if (!_hashHelper.VerifyPassword(request.Password, user.PasswordEncrypted))
                {
                    response.SetError(errorMessage);
                    _logger.LogError(errorMessage);
                }
                else
                {
                    _logger.LogInformation("Generating tokens");
                    response.AccessToken = _tokenHelper.GenerateToken(user);
                }
            }            
            return Task.FromResult(response);
        }
    }
}
