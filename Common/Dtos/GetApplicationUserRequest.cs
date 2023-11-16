using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class GetApplicationUserRequest:IRequest<GetApplicationUserResponseDto>
    {
        [Required]
        public Guid UserGuid { get; set; }
    }
}
