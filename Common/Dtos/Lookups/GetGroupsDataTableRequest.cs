using MediatR;

namespace Common.Dtos
{
	public class GetGroupsDataTableRequest: DataTableRequestBase, IRequest<DataTableResponseBase<GroupLookupRowDto>>
	{
	}
}
