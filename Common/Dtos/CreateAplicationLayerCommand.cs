using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class CreateAplicationLayerCommand:IRequest<ApplicationLayerResponseDto>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int OwnerId { get; set; }
    }
}
