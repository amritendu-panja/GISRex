using Common.Entities;
using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IApplicationPartnerOrganizationRepository: IRepository<ApplicationPartnerOrganization>
    {
        bool IsPartnerNameExists(string organizationName);
        bool IsDomainExists(string domainName);
        Task<List<ApplicationPartnerListItemBase>> GetMostRecentPartners(string query, int count);

       IQueryable<ApplicationPartnerOrganization> FindWithDetails(Expression<Func<ApplicationPartnerOrganization, bool>> expression);
    }
}
