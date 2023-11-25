using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities.Social
{
    [Table("SocialMessage", Schema = "social")]
    public class SocialMessage
    {
        public Guid MessageId { get; private set; }
        public string MessageTitle { get; private set; }
        public string MessageBody { get; private set; }
        public int AuthorId { get; private set; }
        public int? OrganizationId { get; private set; }
        public bool IsVisible { get; private set; }
        public int? LockedById { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public DateTime LockDate { get; private set; }
        public Guid TargetFeedId { get; private set; }
        public int? ReceiverId { get; private set; }

        public ApplicationUser Author { get; set; }
        public ApplicationUser LockedByUser { get; set; }
        public ApplicationUser Receiver {  get; set; }
        public ApplicationPartnerOrganization Organization { get; set; }

        public Feed TargetFeed { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Reaction> Reactions { get; set; }

        protected SocialMessage()
        {
        }

        public SocialMessage(string messageTitle, string messageBody, int authorId, int? organizationId, int? receiverId, Guid targetFeedId)
        {
            MessageTitle = messageTitle;
            MessageBody = messageBody;
            AuthorId = authorId;
            OrganizationId = organizationId;
            ReceiverId = receiverId;
            TargetFeedId = targetFeedId;
            CreateDate = DateTime.UtcNow;
        }

        public void Update(string messageBody)
        {
            MessageBody = messageBody;
            CreateDate = CreateDate.ToUniversalTime();
            UpdateDate = DateTime.UtcNow;
        }

        public void Hide(int lockedById)
        {
            IsVisible = false;
            CreateDate = CreateDate.ToUniversalTime();
            UpdateDate = DateTime.UtcNow;
            LockedById = lockedById;
            LockDate = DateTime.UtcNow;
        }
    }
}
