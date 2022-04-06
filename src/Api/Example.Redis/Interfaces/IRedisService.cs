namespace Example.Redis.Interfaces
{
    public interface IRedisService
    {
        Task<T> GetAsync<T>(string key);
        Task<bool> AddAsync(string key, object value, TimeSpan expiresAt);
        Task<long> AddOnListAsync(string key, object value);        
        Task<IEnumerable<T>> ListAsync<T>(string prefixKey, string key);
        Task<long> RemoveAsync(string key, object value);
        Task<T> RemoveFromListAsync<T>(string prefixKey, string key, int id);
        Task KeyExpireAsync(string prefixKey, string key, DateTime date);
    }
}
