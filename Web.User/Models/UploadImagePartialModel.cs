using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
	public class UploadImagePartialModel
	{
		[Required]
		public required string ImageDataStorageId { get; set; }
		[Required]
		public required string ImageFilenameId { get; set; }
		public string? JsCallBackFunctionName { get; set; }
	}
}
