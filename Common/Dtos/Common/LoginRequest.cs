using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class LoginRequest: IRequest<LoginResponseDto>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
