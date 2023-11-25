using Common.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class SocialReactionMapping : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasKey(t => t.ReactionId);
            builder.Property(t => t.ReactionId).ValueGeneratedOnAdd();
            builder.Property(t => t.ReactionLookupId).IsRequired();
            builder.Property(t => t.AuthorId).IsRequired();
            builder.Property(t => t.IsVisible).IsRequired();

            builder.HasOne(t => t.Author)
                .WithMany(u => u.Reactions)
                .HasForeignKey(t => t.AuthorId);

            builder.HasOne(t => t.ReactionLookup)
                .WithMany(r => r.Reactions)
                .HasForeignKey(t => t.ReactionLookupId);

            builder.HasOne(t => t.TargetMessage)
                .WithMany(m => m.Reactions)
                .HasForeignKey(t => t.TargetMessageId);

            builder.HasOne(t => t.TargetComment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(t => t.TargetCommentId);

        }
    }
}
