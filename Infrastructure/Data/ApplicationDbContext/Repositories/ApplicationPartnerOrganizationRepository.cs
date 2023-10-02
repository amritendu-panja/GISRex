using Application.Repository;
using Common.Entities;
using Infrastructure.Data.Repository;

namespace Infrastructure.Data.ApplicationDbContext.Repositories
{
    public class ApplicationPartnerOrganizationRepository : Repository<ApplicationPartnerOrganization>, IApplicationPartnerOrganizationRepository
    {
        public ApplicationPartnerOrganizationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool IsPartnerNameExists(string organizationName)
        {
            return Find(u => u.OrganizationName == organizationName).Any();
        }
    }
}
