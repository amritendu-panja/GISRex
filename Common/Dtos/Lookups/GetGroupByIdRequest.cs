using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class GetGroupByIdRequest: IRequest<GroupLookupResponseDto>
    {
        [Required]
        public int GroupId { get; set; }
    }
}
