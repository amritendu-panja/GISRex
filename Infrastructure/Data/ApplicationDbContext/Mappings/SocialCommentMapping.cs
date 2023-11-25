using Common.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class SocialCommentMapping : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(t => t.CommentId);
            builder.Property(t => t.CommentId).ValueGeneratedOnAdd();
            builder.Property(t => t.CommentBody).IsRequired();            
            builder.Property(t => t.AuthorId).IsRequired();
            builder.Property(t => t.IsVisible).IsRequired();

            builder.HasOne(t => t.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(t => t.AuthorId);

            builder.HasOne(t => t.TargetMessage)
                .WithMany(r => r.Comments)
                .HasForeignKey(t => t.TargetMessageId);

            builder.HasOne(t => t.LockedByUser)
                .WithMany(u => u.LockedComments)
                .HasForeignKey(t => t.LockedById);

            builder.HasMany(t => t.SubComments)
                .WithOne(c => c.TargetComment)
                .HasForeignKey(t => t.TargetCommentId);
        }
    }
}
