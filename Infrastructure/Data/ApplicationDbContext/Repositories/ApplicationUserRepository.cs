﻿using Application.Repository;
using Common.Entities;
using Dapper;
using Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.ApplicationDbContext.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool IsEmailExists(string email)
        {
            return Find(u => u.Email.ToLower() == email).Any();
        }

        public bool IsUsernameExists(string username)
        {
            return Find(u => u.UserName.ToLower() == username).Any();
        }

        public IQueryable<ApplicationUser> FindWithDetails(Expression<Func<ApplicationUser, bool>> expression)
        {
            return _context.Set<ApplicationUser>()
                .Where(expression)
                .Include(u => u.UserDetails)
                .Include(u => u.Role)
                .Include(u => u.Group);
        }

        public IQueryable<ApplicationUser> FindWithOrganizationDetails(Expression<Func<ApplicationUser, bool>> expression)
        {
            return _context.Set<ApplicationUser>()
                .Where(expression)
                .Include(u => u.UserDetails)
                .Include(u => u.PartnerOrganization)
                .Include(u => u.Role)
                .Include(u => u.Group);
        }

        public async Task<List<ApplicationUserListItemBase>> GetMostRecentOpenedApplicationUsers(string query, int count)
		{
			using (var conn = _context.Database.GetDbConnection())
			{
				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("p_count", count);
				var records = await conn.QueryAsync<ApplicationUserListItemBase>(query, parameters);
				return records.ToList();
			}
		}

        public async Task<List<OrganizationUserListItemBase>> GetMostRecentOpenedOrganizationUsers(string query, int organizationId, int count)
        {
            using (var conn = _context.Database.GetDbConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("org_id", organizationId);
                parameters.Add("p_count", count);
                var records = await conn.QueryAsync<OrganizationUserListItemBase>(query, parameters);
                return records.ToList();
            }
        }

        public IQueryable<ApplicationUser> FindWithRole(Expression<Func<ApplicationUser, bool>> expression)
        {
            return _context.Set<ApplicationUser>()
                .Where(expression)
                .Include(u => u.Role)
                .Include(u => u.Group);
        }
    }
}
