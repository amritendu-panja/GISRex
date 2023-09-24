namespace Application.Helpers
{
    public interface ICacheHelper
    {
        void Set<T>(string key, T value);
        T Get<T>(string key);
        void Remove(string key);
    }
}
