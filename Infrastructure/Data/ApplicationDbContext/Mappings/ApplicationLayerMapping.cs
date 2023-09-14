using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class ApplicationLayerMapping : IEntityTypeConfiguration<ApplicationLayer>
    {
        public void Configure(EntityTypeBuilder<ApplicationLayer> builder)
        {
            builder.HasKey(u => u.LayerId);
            builder
               .HasOne(u => u.Owner)
               .WithMany(u => u.ApplicationLayers)
               .HasForeignKey(u => u.OwnerId)
               .IsRequired();
                
        }
    }
}
