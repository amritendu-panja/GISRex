using MediatR;

namespace Common.Dtos
{
    public class GetOrganizationsDataTableRequest: DataTableRequestBase, IRequest<DataTableResponseBase<GetOrganizationResponseRowDto>>
    {
    }
}
