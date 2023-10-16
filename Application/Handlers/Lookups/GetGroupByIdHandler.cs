using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Mappings;
using Common.Settings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Lookups
{
    public class GetGroupByIdHandler : IRequestHandler<GetGroupByIdRequest, GroupLookupResponseDto>
    {
        private readonly ILogger<GetGroupByIdHandler> _logger;
        private readonly IQueryRepository<ApplicationGroupLookup> _repository;
        private readonly SharedMapping _mapping;

        public GetGroupByIdHandler(ILogger<GetGroupByIdHandler> logger, IQueryRepository<ApplicationGroupLookup> repository, SharedMapping mapping)
        {
            _logger = logger;
            _repository = repository;
            _mapping = mapping;
        }

        public async Task<GroupLookupResponseDto> Handle(GetGroupByIdRequest request, CancellationToken cancellationToken)
        {
            GroupLookupResponseDto responseDto = new GroupLookupResponseDto();
            try
            {
                var groupList = _repository.Find(g => g.GroupName != Constants.SuperAdminGroupName && g.GroupId == request.GroupId);
                if (groupList != null && groupList.Count() > 0)
                {
                    _mapping.Map(groupList, responseDto);
                }
                else
                {
                    string message = $"No groups found with Id: {request.GroupId}";
                    _logger.LogError(message);
                    responseDto.SetError(message);
                }
                return responseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting group with Id: {0}", request.GroupId);
                throw new DbException(ex.Message);
            }
        }
    }
}
