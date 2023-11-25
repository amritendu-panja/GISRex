using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class CreateApplicationUserCommand: IRequest<GetApplicationUserResponseDto>
    {
        [Required]
        public required string UserName { get; set; }
        [Required] 
        public required string Firstname { get; set; }
        [Required] 
        public required string Lastname { get; set; }
        [Required]
        public required string PasswordSalt { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public int Role { get; set; }
    }
}
