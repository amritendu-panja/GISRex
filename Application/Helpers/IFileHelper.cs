namespace Application.Helpers
{
    public interface IFileHelper
    {
        Task<string> GetFileContent(string path);
        string GetCommandForDapper(string fileName);
    }
}
