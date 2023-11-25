using Common.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class ReactionLookupMapping : IEntityTypeConfiguration<ReactionLookup>
    {
        public void Configure(EntityTypeBuilder<ReactionLookup> builder)
        {
            builder.HasKey(t => t.ReactionLookupId);
            builder.Property(t => t.ReactionLookupId).ValueGeneratedOnAdd();

            builder.Property(t => t.ReactionName).IsRequired();
            builder.Property(t => t.ReactionLogo).IsRequired();

            builder.HasMany(t => t.Reactions)
                .WithOne(r => r.ReactionLookup)
                .HasForeignKey(t => t.ReactionLookupId);
        }
    }
}
