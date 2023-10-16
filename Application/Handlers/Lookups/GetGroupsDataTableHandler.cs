using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Extensions;
using Common.Mappings;
using Common.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Lookups
{
	public class GetGroupsDataTableHandler : IRequestHandler<GetGroupsDataTableRequest, DataTableResponseBase<GroupLookupRowDto>>
	{
		private readonly ILogger<GetGroupsDataTableHandler> _logger;
		private readonly IQueryRepository<ApplicationGroupLookup> _repository;
		private readonly SharedMapping _mapping;

		public GetGroupsDataTableHandler(ILogger<GetGroupsDataTableHandler> logger, IQueryRepository<ApplicationGroupLookup> repository, SharedMapping mapping)
		{
			_logger = logger;
			_repository = repository;
			_mapping = mapping;
		}

		public async Task<DataTableResponseBase<GroupLookupRowDto>> Handle(GetGroupsDataTableRequest request, CancellationToken cancellationToken)
		{
			DataTableResponseBase<GroupLookupRowDto> response = new DataTableResponseBase<GroupLookupRowDto>();
			response.Data = new List<GroupLookupRowDto>();
			response.Draw = request.Draw;
			try
			{
				IQueryable<ApplicationGroupLookup> groupList = _repository.Find(g => g.GroupName != Constants.SuperAdminGroupName); ;
				if (request.SearchValue != null && !string.IsNullOrEmpty(request.SearchValue))
				{
					var searchPattern = $"%{request.SearchValue.ToLower()}%";
					groupList = groupList.Where(g => EF.Functions.Like(g.GroupName.ToLower(), searchPattern) || 
					((g.Description != null && !string.IsNullOrEmpty(g.Description)) ? EF.Functions.Like(g.Description.ToLower(), searchPattern) : false));
				}

				if (request.SortDirection != null && request.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
				{
					groupList = groupList.OrderByColumnDescending(request.SortColumn ?? "GroupName");
				}
				else
				{
					groupList = groupList.OrderByColumn(request.SortColumn ?? "GroupName");
				}
				int skipValue = request.Start ?? 0;
				response.RecordsTotal = groupList.Count();
				groupList = groupList.Skip(skipValue).Take(request.PageSize);
				response.RecordsFiltered = groupList.Count();
				if (groupList != null && groupList.Count() > 0)
				{
					foreach (var group in groupList.ToList())
					{
						GroupLookupRowDto dto = new GroupLookupRowDto();
						_mapping.Map(group, dto);
						response.Data.Add(dto);
					}
				}
				else
				{
					string message = "No groups found";
					_logger.LogError(message);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "No groups found");
			}
			return response;
		}
	}
}
