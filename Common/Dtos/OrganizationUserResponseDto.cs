using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class OrganizationUserResponseDto: ApplicationUserResponseDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set;}
        public string OrganizationLogo { get; set;}
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsPasswordExpired { get; set; }
    }
}
