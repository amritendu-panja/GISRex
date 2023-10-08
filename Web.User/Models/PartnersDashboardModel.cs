using Common.Dtos;

namespace Web.User.Models
{
    public class PartnersDashboardModel
    {
        public PartnersDashboardModel()
        {
            PartnerMruList = new List<BaseApplicationPartnerListItemDto>();
        }

        public List<BaseApplicationPartnerListItemDto> PartnerMruList { get; set; }
    }
}
