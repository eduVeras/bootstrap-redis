using Example.Redis.Interfaces;
using StackExchange.Redis;

namespace Example.Redis.Services
{
    public class RedisService : IRedisService
    {
        private readonly ILogger<RedisService> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;



        public RedisService(ILogger<RedisService> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _connectionMultiplexer = connectionMultiplexer;
            
            _connectionMultiplexer.ConnectionRestored += OnConnectionRestored;
            _connectionMultiplexer.ConnectionFailed += OnConnectionFailed;
            _connectionMultiplexer = connectionMultiplexer;
        }

        private void OnConnectionRestored(object? sender, ConnectionFailedEventArgs e)
        {
            _logger.LogWarning(e.Exception, "On restoring connection to Redis {RedisEndpoint} via {RedisConnectionType}, failure {RedisFailureType}.",
                e.EndPoint,
                e.ConnectionType,
                e.FailureType);
        }

        private void OnConnectionFailed(object? sender, ConnectionFailedEventArgs e)
        {
            _logger.LogCritical(e.Exception, "On connecting to Redis {RedisEndpoint} via {RedisConnectionType}, failure {RedisFailureType}.",
                e.EndPoint,
                e.ConnectionType,
                e.FailureType);
        }

        public Task<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(string key, object value, TimeSpan expiresAt)
        {
            throw new NotImplementedException();
        }

        public Task<long> AddOnListAsync(string key, object value)
        {
            throw new NotImplementedException();
        }

        public Task<long> AddOnListAsync(string prefixKey, string key, object value)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ListAsync<T>(string prefixKey, string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> RemoveAsync(string prefixKey, string key, object value)
        {
            throw new NotImplementedException();
        }

        public Task<T> RemoveFromListAsync<T>(string prefixKey, string key, int id)
        {
            throw new NotImplementedException();
        }

        public Task KeyExpireAsync(string prefixKey, string key, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
