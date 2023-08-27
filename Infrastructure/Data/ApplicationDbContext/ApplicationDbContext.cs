using Common.Entities;
using Infrastructure.Data.ApplicationDbContext.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.ApplicationDbContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserMapping).Assembly);
        }
    }
}
