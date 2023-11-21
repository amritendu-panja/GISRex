using Common.Dtos;

namespace Application.Repository
{
	public interface IDataTableRepository<T>
	where T : class
	{
		Task<DataTableResponseBase<T>> Get(DataTableRequestBase requestBase, string dapperQueryPath, List<DapperParameter>? additionalParams=null);
	}
}
