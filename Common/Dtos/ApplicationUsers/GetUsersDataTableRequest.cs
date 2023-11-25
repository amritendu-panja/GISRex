using MediatR;

namespace Common.Dtos
{
	public class GetUsersDataTableRequest : DataTableRequestBase, IRequest<DataTableResponseBase<GetUserResponseRowDto>>
	{
    }
}
