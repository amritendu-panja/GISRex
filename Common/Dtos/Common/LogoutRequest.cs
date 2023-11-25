using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class LogoutRequest: IRequest<LogoutResponseDto>
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
