using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sunday.Core.Infrastructure;
using Sunday.Core.Model.Options;

namespace Sunday.Core.Extensions
{
    /// <summary>
    /// 注入Kafka相关配置
    /// </summary>
    public static class KafkaSetup
    {
        public static void AddKafkaSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (Appsettings.App(new string[] { "Kafka", "Enabled" }).ObjToBool())
            {
                services.Configure<KafkaOptions>(configuration.GetSection("kafka"));
                services.AddSingleton<IKafkaConnectionPool, KafkaConnectionPool>();
            }
        }
    }
}