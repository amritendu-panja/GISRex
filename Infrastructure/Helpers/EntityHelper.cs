using Application.Helpers;

namespace Infrastructure.Helpers
{
	public class EntityHelper : IEntityHelper
	{
		public string FindColumnNameLike<T>(string columnName)
		{
			string foundColumn = string.Empty;
			var properties = typeof(T).GetType().GetProperties();
			foreach (var property in properties)
			{
				if(property.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
				{
					foundColumn = property.Name;
					break;
				}
			}
			return foundColumn;
		}

		public string FixColumnName(string columnName)
		{
			if (string.IsNullOrEmpty(columnName))
			{
				return string.Empty;
			}
			return $"{columnName[0].ToString().ToUpper()}{columnName.Substring(1)}";
		}

		public string GetFirstColumnName<T>()
		{
			var properties = typeof(T).GetType().GetProperties();
			return properties.First().Name;
		}
	}
}
