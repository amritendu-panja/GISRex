using Common.Entities;

namespace Application.Repository
{
    public interface IApplicationPartnerOrganizationRepository: IRepository<ApplicationPartnerOrganization>
    {
        bool IsPartnerNameExists(string organizationName);
        Task<List<ApplicationPartnerListItemBase>> GetMostRecentPartners(int count);
    }
}
