using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
