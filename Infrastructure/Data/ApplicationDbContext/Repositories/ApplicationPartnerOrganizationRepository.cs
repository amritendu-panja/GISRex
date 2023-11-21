using Application.Repository;
using Common.Entities;
using Dapper;
using Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Dapper.SqlMapper;

namespace Infrastructure.Data.ApplicationDbContext.Repositories
{
    public class ApplicationPartnerOrganizationRepository : Repository<ApplicationPartnerOrganization>, IApplicationPartnerOrganizationRepository
    {
        public ApplicationPartnerOrganizationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool IsPartnerNameExists(string organizationName)
        {
            return base.Find(u => u.OrganizationName == organizationName).Any();
        }

        public async Task<List<ApplicationPartnerListItemBase>> GetMostRecentPartners(string query, int count)
        {
            using (var conn = _context.Database.GetDbConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_count", count);
                var records = await conn.QueryAsync<ApplicationPartnerListItemBase>(query, parameters);
                return records.ToList();
            }
        }

        public IQueryable<ApplicationPartnerOrganization> FindWithDetails(Expression<Func<ApplicationPartnerOrganization, bool>> expression)
        {
            return _context.Set<ApplicationPartnerOrganization>()
                .Where(expression)
                .Include(u => u.User)
                .ThenInclude(u => u.Group)
                .Include(u => u.User)
                .ThenInclude(u => u.Role);
        }

		public bool IsDomainExists(string domainName)
		{
			return base.Find(u => u.DomainName == domainName).Any();
		}
	}
}
