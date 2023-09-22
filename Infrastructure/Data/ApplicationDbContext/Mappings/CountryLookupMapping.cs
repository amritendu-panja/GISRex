using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class CountryLookupMapping : IEntityTypeConfiguration<CountryLookup>
    {
        public void Configure(EntityTypeBuilder<CountryLookup> builder)
        {
            builder.HasKey(e => e.CountryId);
            builder.Property(e => e.CountryId)
                .ValueGeneratedOnAdd();
        }
    }
}
