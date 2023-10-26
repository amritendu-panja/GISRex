using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class ApplicationPartnerOrganizationMapping : IEntityTypeConfiguration<ApplicationPartnerOrganization>
    {
        public void Configure(EntityTypeBuilder<ApplicationPartnerOrganization> builder)
        {
            builder
                .HasKey(u => u.OrganizationId);
            builder
                .Property(u => u.OrganizationId)
                .IsRequired()
                .ValueGeneratedOnAdd();
            builder
                .HasOne(u => u.User)
                .WithOne(u => u.PartnerOrganization)
                .HasForeignKey<ApplicationPartnerOrganization>(u => u.UserId);
            //builder
            //    .HasOne(u => u.Country)
            //    .WithMany(u => u.ApplicationPartners)
            //    .HasForeignKey(u => u.CountryCode);
            //builder
            //    .HasOne(u => u.State)
            //    .WithMany(u => u.ApplicationPartners)
            //    .HasForeignKey(u => u.StateCode);
        }
    }
}
