using Common.Entities;

namespace Application.Repository
{
    public interface IApplicationPartnerOrganizationRepository: IRepository<ApplicationPartnerOrganization>
    {
        bool IsPartnerNameExists(string organizationName);
    }
}
