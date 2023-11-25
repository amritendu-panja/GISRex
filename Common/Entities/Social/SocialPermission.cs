using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities.Social
{
    [Table("SocialPermission", Schema = "social")]
    public class SocialPermission
    {
        public Guid SocialPermissionId { get; private set; }
        public int PermissionId { get; private set; }
        public Guid FeedGuid { get; private set; }
        public int? UserId { get; private set; }
        public int? GroupId { get; private set; }
        public int? OrganizationId { get; private set; }
        public bool IsAllowed { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public int AuthorId { get; private set; }
        public int? UpdatedById { get; private set; }


        public Feed TargetFeed { get; set; }
        public PermissionLookup PermissionLookup { get; set; }
        public ApplicationUser ApplicableUser { get; set; }
        public ApplicationPartnerOrganization ApplicableOrganization { get; set; }
        public ApplicationGroupLookup ApplicableGroup { get; set; }
        public ApplicationUser Author { get; set; }
        public ApplicationUser UpdatedByUser { get; set; }

        public SocialPermission(int permissionId, Guid feedGuid, int? userId, int? groupId, int? organizationId, bool isAllowed, int authorId)
        {
            PermissionId = permissionId;
            FeedGuid = feedGuid;
            UserId = userId;
            GroupId = groupId;
            OrganizationId = organizationId;
            IsAllowed = isAllowed;
            AuthorId = authorId;
            CreateDate = DateTime.UtcNow;
        }

        public void SetAllow(bool allow, int updatedById)
        {
            IsAllowed = allow;
            UpdatedById = updatedById;
            UpdateDate = DateTime.UtcNow;
            CreateDate = CreateDate.ToUniversalTime();
        }
    }
}
