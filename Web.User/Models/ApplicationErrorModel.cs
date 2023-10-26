using Common.Dtos;

namespace Web.User.Models
{
    public class ApplicationErrorModel: BaseResponseDto
    {
        public int StatusCode { get; set; }
    }
}
