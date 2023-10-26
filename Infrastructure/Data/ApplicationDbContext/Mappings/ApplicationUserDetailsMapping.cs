using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class ApplicationUserDetailsMapping : IEntityTypeConfiguration<ApplicationUserDetails>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserDetails> builder)
        {
            builder.HasKey(u => u.UserId);
            builder
               .HasOne(u => u.User)
               .WithOne(u => u.UserDetails)
               .HasForeignKey<ApplicationUser>(u => u.UserId);
            //builder
            //    .HasOne(u => u.Country)
            //    .WithMany(u => u.ApplicationUsersDetails)
            //    .HasForeignKey(u => u.CountryCode);
            //builder
            //    .HasOne(u => u.State)
            //    .WithMany(u => u.ApplicationUsersDetails)
            //    .HasForeignKey(u => u.StateCode);
        }
    }
}
