using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class GetUsersRequest
    {
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
        public string? OrderByColumn { get; set; }
        public string? OrderDirection {  get; set; }
    }
}
