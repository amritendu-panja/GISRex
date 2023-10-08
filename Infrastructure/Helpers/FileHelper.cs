using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetCommandForDapper(string fileName)
        {
            return Path.Combine(typeof(Infrastructure.MediatorDependency.MediatorStartup).Assembly.Location, "Data\\ApplicationDbContext\\DapperQueries", $"{fileName}.sql");
        }

        public Task<string> GetFileContent(string path)
        {
            throw new NotImplementedException();
        }
    }
}
