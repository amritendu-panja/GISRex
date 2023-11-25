namespace Common.Dtos
{
	public class DataTableRequestBase
	{
		public Guid UserGuid { get; set; }
		public int Draw { get; set; }
		public string? SortColumn { get; set; }
		public string? SortDirection { get; set; }
		public int? Start { get; set; }
		public string? SearchValue { get; set; }
		public int PageSize { get; set; }
	}
}
