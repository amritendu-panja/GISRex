using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class GetApplicationUserRequest:IRequest<ApplicationUserResponseDto>
    {
        [Required]
        public Guid UserGuid { get; set; }
    }
}
