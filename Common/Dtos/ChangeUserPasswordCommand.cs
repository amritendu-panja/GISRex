using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class ChangeUserPasswordCommand: IRequest<LogoutResponseDto>
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public required string OldPassword { get; set; }
        [Required]
        public required string NewPassword { get; set; }
    }
}
