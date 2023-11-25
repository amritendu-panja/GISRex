using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities.Social
{
    [Table("SocialFeed", Schema = "social")]
    public class Feed
    {
        public Guid FeedId { get; private set; }
        public string FeedName { get; private set; }
        public int OrganizationId { get; private set; }
        public int GroupId { get; private set; }
        public DateTime CreateDate { get; private set; }
        public bool IsEnabled { get; private set; }
        public int LockedById { get; private set; }
        public int CreatedById { get; private set; }

        public ApplicationUser Creator { get; set; }
        public ApplicationUser LockedByUser { get; set; }
        public ApplicationGroupLookup Group { get; set; }
        public ApplicationPartnerOrganization Organization { get; set; }

        public List<SocialMessage> Messages { get; set; }

        public Feed(string feedName, int organizationId, int createdById)
        {
            FeedName = feedName;
            OrganizationId = organizationId;
            CreatedById = createdById;
            CreateDate = DateTime.UtcNow;
        }

        public Feed(string feedName, int organizationId, int groupId, int createdById)
        {
            FeedName = feedName;
            OrganizationId = organizationId;
            GroupId = groupId;
            CreatedById = createdById;
            CreateDate = DateTime.UtcNow;
        }

        public void LockFeed(int lockedById)
        {
            IsEnabled = false;
            LockedById = lockedById;
            CreateDate = CreateDate.ToUniversalTime();
        }

        protected Feed()
        {
        }
    }
}
