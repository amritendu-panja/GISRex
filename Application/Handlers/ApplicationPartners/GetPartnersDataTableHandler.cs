using Application.Repository;
using Common.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationPartners
{
    public class GetPartnersDataTableHandler : IRequestHandler<GetOrganizationsDataTableRequest, DataTableResponseBase<GetOrganizationResponseRowDto>>
    {
        private readonly ILogger<GetPartnersDataTableHandler> _logger;
        private readonly IDataTableRepository<GetOrganizationResponseRowDto> _repository;

        public GetPartnersDataTableHandler(ILogger<GetPartnersDataTableHandler> logger, IDataTableRepository<GetOrganizationResponseRowDto> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<DataTableResponseBase<GetOrganizationResponseRowDto>> Handle(GetOrganizationsDataTableRequest request, CancellationToken cancellationToken)
        {
            DataTableResponseBase<GetOrganizationResponseRowDto> response = new DataTableResponseBase<GetOrganizationResponseRowDto>();
            response.Data = new List<GetOrganizationResponseRowDto>();
            response.Draw = request.Draw;
            try
            {
                return await _repository.Get(request, "get_application_organizations_datatable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured when getting Partners DataTable.");
            }
            return response;
        }
    }
}
