using Common.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class SocialFeedMapping : IEntityTypeConfiguration<Feed>
    {
        public void Configure(EntityTypeBuilder<Feed> builder)
        {
            builder.HasKey(t => t.FeedId);
            builder.Property(t => t.FeedId).ValueGeneratedOnAdd();
            builder.Property(t => t.CreatedById).IsRequired();
            builder.Property(t => t.FeedName).IsRequired();

            builder.HasOne(t => t.Creator)
                .WithMany(c => c.Feeds)
                .HasForeignKey(t => t.CreatedById);

            builder.HasOne(t => t.LockedByUser)
                .WithMany(c => c.LockedFeeds)
                .HasForeignKey(t => t.LockedById);

            builder.HasOne(t => t.Group)
                .WithMany(g => g.Feeds)
                .HasForeignKey(t => t.GroupId);

            builder.HasOne(t => t.Organization)
                .WithMany(o => o.Feeds)
                .HasForeignKey(t => t.OrganizationId);
            
        }
    }
}
