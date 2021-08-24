using System;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Sunday.Core.Infrastructure;

namespace Sunday.Core.Api.ServiceExtensions
{
    /// <summary>
    /// Redis缓存 启动服务
    /// </summary>
    public static class RedisCacheSetup
    {
        public static void AddRedisCacheSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IRedisHelper, RedisHelper>();

            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton(sp =>
               {
                   //获取连接字符串
                   string redisConfiguration = Appsettings.App(new string[] { "Redis", "ConnectionString" });

                   ConfigurationOptions configuration = ConfigurationOptions.Parse(redisConfiguration, true);

                   configuration.ResolveDns = true;

                   return ConnectionMultiplexer.Connect(configuration);
               });
        }
    }
}