using Application.Helpers;
using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Data.Repository
{
	public class DataTableRepository<T> : IDataTableRepository<T> where T : class
	{
		private readonly ApplicationDbContext.ApplicationDbContext _context;
		private readonly IFileHelper _fileHelper;
		private readonly IEntityHelper _entityHelper;
		private readonly ILogger<DataTableRepository<T>> _logger;

		public DataTableRepository(ApplicationDbContext.ApplicationDbContext context, 
			IFileHelper fileHelper, 
			IEntityHelper entityHelper,
			ILogger<DataTableRepository<T>> logger)
		{
			_context = context;
			_fileHelper = fileHelper;
			_entityHelper = entityHelper;
			_logger = logger;
		}

		public async Task<DataTableResponseBase<T>> Get(DataTableRequestBase requestBase, string queryName)
		{
			DataTableResponseBase<T> response = new DataTableResponseBase<T>();
			string sortColumn = string.Empty;
			if(requestBase.SortColumn != null && !string.IsNullOrEmpty(requestBase.SortColumn))
			{
				sortColumn = _entityHelper.FixColumnName(requestBase.SortColumn);
			}
			else
			{
				sortColumn = _entityHelper.GetFirstColumnName<T>();
			}
			if (!string.IsNullOrEmpty(sortColumn))
			{
				string query = await _fileHelper.GetFileContent(queryName);
				query = string.Format(query, sortColumn, requestBase.SortDirection);
				if (!string.IsNullOrEmpty(requestBase.SearchValue))
				{
					requestBase.SearchValue = $"%{requestBase.SearchValue.ToLower()}%";
				}
				using (var connection = _context.Database.GetDbConnection())
				{
					using (var multi = await connection.QueryMultipleAsync(query, requestBase))
					{
						response.RecordsTotal = await multi.ReadFirstAsync<int>();
						response.RecordsFiltered = await multi.ReadFirstAsync<int>();
						var data = await multi.ReadAsync<T>();
						if(data != null && data.Count() > 0) 
						{
							response.Data = new List<T>();
							response.Data.AddRange(data);
						}						
					}
				}
			}
			else
			{
				throw new DbException("Sort Column not specified");
			}
			return response;
		}
	}
}
