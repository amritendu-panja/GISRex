using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class FindByUsernameRequest: IRequest<ApplicationUserResponseDto>
    {
        [Required]
        public required string Username { get; set; }
    }
}
