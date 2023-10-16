using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
	[Table("ApplicationGroupLookup", Schema = "lookups")]
	public class ApplicationGroupLookup
	{
		public int GroupId { get; private set; }
		public string GroupName { get; private set; }
		public string? Description { get; private set; }
		public DateTime? CreatedDate { get; private set; }
		public DateTime? ModifiedDate { get; private set; }
	}
}
