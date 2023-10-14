using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
	public class CountryStatePartialModel
	{
		[Required]
		/// <summary>
		/// Id of country select dropdown
		/// </summary>
		public required string CountrySelectId { get; set; }
		public string? SelectedCountryCode { get; set;}
		[Required]
		/// <summary>
		/// Id of state select dropdown
		/// </summary>
		public required string StateSelectId { get; set; }
		public string? SelectedStateCode { get; set;}
		public bool NeedCountryCallingCode { get; set; }
		/// <summary>
		/// Id of country calling code show component
		/// </summary>
		public string? CountryCallingCodeId { get; set; }
	}
}
