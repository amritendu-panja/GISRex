using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class GetApplicationLayerRequest: IRequest<ApplicationLayerResponseDto>
    {
        public Guid Id { get; set; }
    }
}
