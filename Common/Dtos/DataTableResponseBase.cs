namespace Common.Dtos
{
	public class DataTableResponseBase<T>
	{
		public int Draw { get; set; }
		public int RecordsTotal { get; set; }
		public int RecordsFiltered { get; set; }
		public List<T> Data { get; set; } = new List<T>();
	}
}
