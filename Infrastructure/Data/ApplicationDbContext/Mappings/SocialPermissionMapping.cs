using Common.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class SocialPermissionMapping : IEntityTypeConfiguration<SocialPermission>
    {
        public void Configure(EntityTypeBuilder<SocialPermission> builder)
        {
            builder.HasKey(t => t.SocialPermissionId);
            builder.Property(t => t.SocialPermissionId).ValueGeneratedOnAdd();
            builder.Property(t => t.AuthorId).IsRequired();
            builder.Property(t => t.IsAllowed).IsRequired();

            builder.HasOne(t => t.PermissionLookup)
                .WithMany(s => s.SocialPermissions)
                .HasForeignKey(t => t.PermissionId);

            builder.HasOne(t => t.ApplicableUser)
                .WithMany(u => u.SocialPermissions)
                .HasForeignKey(t => t.UserId);

            builder.HasOne(t => t.ApplicableGroup)
                .WithMany(g => g.SocialPermissions)
                .HasForeignKey(t => t.GroupId);

            builder.HasOne(t => t.ApplicableOrganization)
                .WithMany(o => o.SocialPermissions)
                .HasForeignKey(t => t.OrganizationId);

            builder.HasOne(t => t.Author)
                .WithMany(a => a.AuthoredSocialPermissions)
                .HasForeignKey(t => t.AuthorId);

            builder.HasOne(t => t.UpdatedByUser)
                .WithMany(u => u.UpdatedSocialPermissions)
                .HasForeignKey(t => t.UpdatedById);
        }
    }
}
