using Common.Dtos;

namespace Web.User.Models
{
    public class PartnersDashboardModel
    {
        public PartnersDashboardModel()
        {
            PartnerMruList = new List<BaseApplicationOrganizationListItemDto>();
        }

        public List<BaseApplicationOrganizationListItemDto> PartnerMruList { get; set; }
    }
}
