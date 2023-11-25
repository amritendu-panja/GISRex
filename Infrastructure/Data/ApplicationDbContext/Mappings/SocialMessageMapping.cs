using Common.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class SocialMessageMapping : IEntityTypeConfiguration<SocialMessage>
    {
        public void Configure(EntityTypeBuilder<SocialMessage> builder)
        {
            builder.HasKey(t => t.MessageId);
            builder.Property(t => t.MessageId).ValueGeneratedOnAdd();
            builder.Property(t => t.MessageTitle).IsRequired();
            builder.Property(t => t.MessageBody).IsRequired();
            builder.Property(t => t.IsVisible).IsRequired();
            builder.Property(t => t.AuthorId).IsRequired();
            builder.Property(t => t.TargetFeedId).IsRequired();

            builder.HasOne(t => t.Author)
                .WithMany(a => a.AuthoredMessages)
                .HasForeignKey(t => t.AuthorId);

            builder.HasOne(t => t.TargetFeed)
                .WithMany(f => f.Messages)
                .HasForeignKey(t => t.TargetFeedId);

            builder.HasOne(t => t.LockedByUser)
                .WithMany(u => u.LockedMessages)
                .HasForeignKey(t => t.LockedById);

            builder.HasOne(t => t.Organization)
                .WithMany(o => o.SocialMessages)
                .HasForeignKey(t => t.OrganizationId);

            builder.HasOne(t => t.Receiver)
                .WithMany(r => r.ReceivedMessages)
                .HasForeignKey(t => t.ReceiverId);

        }
    }
}
