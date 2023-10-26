using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class ApplicationGroupModel
    {
        public int GroupId { get; set; }
        [Display(Name = "Name")]
        public string GroupName { get; set; }
        public string? Description { get; set; }
    }
}
