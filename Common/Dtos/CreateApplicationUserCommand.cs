using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class CreateApplicationUserCommand: IRequest<ApplicationUserResponseDto>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordSalt { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
