namespace Application.Helpers
{
	public interface IEntityHelper		
	{
		string GetFirstColumnName<T>();
		public string FindColumnNameLike<T>(string columnName);
		public string FixColumnName(string columnName);
	}
}
