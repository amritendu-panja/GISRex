using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class ApplicationPartnerListItemBase
    {
        public int OrganizationId { get; private set; }
        public string OrganizationName { get; private set; }
        public string? LogoUrl { get; private set; }
        public string CountryCode { get; private set; }
    }
}
