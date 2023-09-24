using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("UserRoleLookup", Schema = "lookups")]
    public class UserRoleLookup
    {
        public int RoleId { get; private set; }
        public string Role { get; private set; }
        public string Description { get; private set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
