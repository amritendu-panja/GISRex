using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class BaseResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public BaseResponseDto() 
        {
            Success = true;
        }

        public void SetSuccess()
        {
            Success = true;
        }

        public void SetError (string message)
        {
            Success = false;
            Message = message;
        }
    }
}
