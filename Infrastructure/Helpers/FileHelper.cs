using Application.Helpers;

namespace Infrastructure.Helpers
{
	public class FileHelper : IFileHelper
    {
        public string GetCommandForDapper(string fileName)
        {
            return Path.Combine("DapperQueries", $"{fileName}.sql");
        }

        public async Task<string> GetFileContent(string fileName)
        {
            var path = GetCommandForDapper(fileName);
            return await File.ReadAllTextAsync(path);
        }
    }
}
