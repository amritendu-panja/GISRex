using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities.Social
{
    [Table("SocialReaction", Schema = "social")]
    public class Reaction
    {
        public Guid ReactionId { get; private set; }
        public int ReactionLookupId { get; private set; }
        public Guid? TargetMessageId { get; private set; }
        public Guid? TargetCommentId { get; private set; }
        public DateTime CreateDate { get; private set; }
        public bool IsVisible { get; private set; }
        public int AuthorId { get; private set; }

        public ApplicationUser Author { get; set; }
        public SocialMessage TargetMessage { get; set; }
        public Comment TargetComment { get; set; }
        public ReactionLookup ReactionLookup { get; set; }

        protected Reaction()
        {
        }

        public Reaction(int reactionLookupId, Guid? targetMessageId, Guid? targetCommentId, int authorId)
        {
            ReactionLookupId = reactionLookupId;
            TargetMessageId = targetMessageId;
            TargetCommentId = targetCommentId;
            AuthorId = authorId;
            CreateDate = DateTime.UtcNow;
        }

        public void SetVisible(bool isVisible)
        {
            IsVisible = isVisible;
            CreateDate = CreateDate.ToUniversalTime();
        }
    }
}
