using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class FindByUsernameRequest: IRequest<GetApplicationUserResponseDto>
    {
        [Required]
        public required string Username { get; set; }
    }
}
