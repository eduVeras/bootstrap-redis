using Example.Redis.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Example.Redis.Services
{
    public class RedisService : IRedisService
    {
        private readonly ILogger<RedisService> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _db;


        public RedisService(ILogger<RedisService> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _connectionMultiplexer = connectionMultiplexer;

            _db = _connectionMultiplexer.GetDatabase();

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

        public async Task<T> GetAsync<T>(string key)
        {
            _logger.LogInformation("{MethodName} Getting {RedisKey}",
                nameof(GetAsync),
                key);

            var value = await _db.StringGetAsync(key).ConfigureAwait(false);

            return value.IsNull ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> AddAsync(string key, object value, TimeSpan expiresAt)
        {
            _logger.LogInformation("{MethodName} Adding {RedisKey} with Expiration {Redis.Expiration}", nameof(AddAsync), key, expiresAt);

            var obj = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return await _db.StringSetAsync(key, new RedisValue(obj), expiresAt).ConfigureAwait(false);
        }

        public async Task<long> AddOnListAsync(string key, object value)
        {
            _logger.LogInformation("{MethodName} Adding On List {RedisKey}", nameof(AddOnListAsync), key);

            var obj = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return await _db.ListLeftPushAsync(key, new RedisValue(obj)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> ListAsync<T>(string prefixKey, string key)
        {
            _logger.LogInformation("{MethodName} Adding {RedisKey}", nameof(ListAsync), key);

            var values = await _db.ListRangeAsync($"{prefixKey}:{key}", 0, -1).ConfigureAwait(false);

            return Array.ConvertAll(values, value => JsonConvert.DeserializeObject<T>(value)).ToList();
        }

        public async Task<long> RemoveAsync(string prefixKey, string key, object value)
        {
            _logger.LogInformation("{MethodName} Removing key {RedisKey}", nameof(RemoveAsync), key);

            return await _db.ListRemoveAsync($"{prefixKey}:{key}", new RedisValue(JsonConvert.SerializeObject(value))).ConfigureAwait(false);
        }

        public async Task<T> RemoveFromListAsync<T>(string prefixKey, string key, int id)
        {
            _logger.LogInformation("{MethodName} Removing key {RedisKey}", nameof(RemoveAsync), key);

            var result = await SearchElementByIdAsync<T>(prefixKey, key, id).ConfigureAwait(false);

            if (!object.Equals(result.Item1, default(T)))
            {
                var value = new RedisValue(result.Item2);

                await _db.ListRemoveAsync($"{prefixKey}:{key}", value).ConfigureAwait(false);

                return result.Item1;
            }

            return default(T);
        }

        private async Task<(T, string)> SearchElementByIdAsync<T>(string prefixKey, string key, int id)
        {
            var values = await _db.ListRangeAsync($"{prefixKey}:{key}", 0, -1).ConfigureAwait(false);

            for (int i = 0; i < values.Length; i++)
            {
                var a = values[i].ToString();

                if (a.ToUpper().Contains($"\"ID\":{id}") || a.ToUpper().Contains($"\"ID\": {id}"))
                {
                    var obj = values[i];

                    return (JsonConvert.DeserializeObject<T>(obj), a);
                }
            }

            return (default(T), string.Empty);
        }

        public async Task KeyExpireAsync(string prefixKey, string key, DateTime date)
        {
            await _db.KeyExpireAsync(new RedisKey($"{prefixKey}:{key}"), date).ConfigureAwait(false);
        }
    }
}
