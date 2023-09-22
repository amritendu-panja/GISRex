using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class StateLookupMapping : IEntityTypeConfiguration<StateLookup>
    {
        public void Configure(EntityTypeBuilder<StateLookup> builder)
        {
            builder.HasKey(e => e.StateUniqueId);
            builder.Property(e => e.StateUniqueId)
                .ValueGeneratedOnAdd();
        }
    }
}
