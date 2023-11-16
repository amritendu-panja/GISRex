using Common.Dtos;

namespace Web.User.Models
{
    public class AdminLandingModel
    {
        public List<BaseApplicationOrganizationListItemDto> PartnerMruList { get; set; }
        public List<BaseApplicationUserListItemDto> UserMruList { get; set; }
    }
}
