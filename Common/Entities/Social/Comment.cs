using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities.Social
{
    [Table("SocialComment", Schema = "social")]
    public class Comment
    {
        public Guid CommentId { get; private set; }
        public string CommentBody { get; private set; }
        public Guid? TargetMessageId { get; private set; }
        public Guid? TargetCommentId { get; private set; }
        public DateTime CreateDate { get; private set; }
        public bool IsVisible { get; private set; }
        public DateTime LockDate { get; private set; }
        public int AuthorId { get; private set; }
        public int? LockedById { get; private set; }

        public ApplicationUser Author { get; set; }
        public ApplicationUser LockedByUser { get; set; }
        public SocialMessage TargetMessage { get; set; }
        public List<Reaction> Reactions { get; set; }
        public Comment TargetComment { get; set; }
        public List<Comment> SubComments { get; set; }

        protected Comment()
        {
        }

        public Comment(string commentBody, Guid? targetMessageId, Guid? targetCommentId, int authorId)
        {
            CommentBody = commentBody;
            TargetMessageId = targetMessageId;
            TargetCommentId = targetCommentId;
            AuthorId = authorId;
            CreateDate = DateTime.UtcNow;
        }

        public void LockComment(int lockedById)
        {
            IsVisible = false;
            LockedById = lockedById;
            LockDate = DateTime.UtcNow;
            CreateDate = CreateDate.ToUniversalTime();
        }
    }
}
