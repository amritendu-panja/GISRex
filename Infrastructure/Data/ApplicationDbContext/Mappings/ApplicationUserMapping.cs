using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserId)
                .ValueGeneratedOnAdd();
            builder
                .HasMany(u => u.ApplicationLayers)
                .WithOne(u => u.Owner)
                .HasForeignKey(u=> u.OwnerId);

            builder
                .HasMany(u => u.SecurityTokenLogs)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            builder
                .HasOne(u => u.UserDetails)
                .WithOne(u => u.User)
                .HasForeignKey<ApplicationUser>(u => u.UserId);

            builder
                .HasOne(u => u.Role)
                .WithMany(u => u.ApplicationUsers)
                .HasForeignKey(u => u.RoleId);
        }
    }
}
