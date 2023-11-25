using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class PermissionLookupMapping : IEntityTypeConfiguration<PermissionLookup>
    {
        public void Configure(EntityTypeBuilder<PermissionLookup> builder)
        {
            builder.HasKey(t => t.PermissionId);
            builder.Property(t => t.PermissionId).ValueGeneratedOnAdd();

            builder.Property(t =>t.PermissionName).IsRequired();
        }
    }
}
