using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
	public class CheckDomainExistsRequest: IRequest<BaseResponseDto>
	{
		[Required]
		public string Domain {  get; set; }
	}
}
