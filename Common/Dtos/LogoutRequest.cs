using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class LogoutRequest: IRequest<LogoutResponseDto>
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
