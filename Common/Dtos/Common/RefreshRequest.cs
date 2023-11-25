using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class RefreshRequest : IRequest<LoginResponseDto>
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
