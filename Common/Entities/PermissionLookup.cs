using Common.Entities.Social;

namespace Common.Entities
{
    public class PermissionLookup
    {
        public int PermissionId { get; private set; }
        public string PermissionName { get; private set; }

        public List<SocialPermission> SocialPermissions { get; set; }
    }
}
