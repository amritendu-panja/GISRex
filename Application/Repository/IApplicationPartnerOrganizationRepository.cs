using Common.Entities;
using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IApplicationPartnerOrganizationRepository: IRepository<ApplicationPartnerOrganization>
    {
        bool IsPartnerNameExists(string organizationName);
        Task<List<ApplicationPartnerListItemBase>> GetMostRecentPartners(int count);

       IQueryable<ApplicationPartnerOrganization> FindWithDetails(Expression<Func<ApplicationPartnerOrganization, bool>> expression);
    }
}
