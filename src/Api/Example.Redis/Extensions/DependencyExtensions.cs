using StackExchange.Redis;

namespace Example.Redis.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration config)
        {
            var multiplexer = ConnectionMultiplexer.Connect(config.GetConnectionString("Redis"));
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            return services;
        }

    }
}
