using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sunday.Core.Extensions.Apollo;
using Microsoft.Extensions.Logging;
using System;
using Sunday.Core.Extensions.ConfigurationExtend;
using System.Collections.Generic;

namespace Sunday.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
               .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                     {
                         config.Sources.Clear();
                         config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                               .AddJsonFile($"appsettings{ GetAppSettingsConfigName() }json", optional: true, reloadOnChange: false);

                         //自定义配置源
                         config.AddCustomConfiguration(option =>
                         {
                             option.LogTag = "This is CustomConfiguration";
                             option.DataChangeAction = (key,value) =>
                             {
                                 value += "t";
                             };
                             option.DataInitFunc = () => 
                             {
                                 Dictionary<string, string> dic = new Dictionary<string, string>()
                                 {
                                 };

                                 return dic;
                             };
                         });
                         //接入Apollo配置中心
                         config.AddConfigurationApollo("appsettings.apollo.json");
                     })
                      .UseUrls("http://*:8081")
                      .ConfigureLogging((hostingContext, builder) =>
                      {
                          // 1.过滤掉系统默认的一些日志
                          builder.AddFilter("System", LogLevel.Error);
                          builder.AddFilter("Microsoft", LogLevel.Error);

                          // 2.也可以在appsettings.json中配置，LogLevel节点

                          // 3.统一设置
                          builder.SetMinimumLevel(LogLevel.Error);

                          // 默认log4net.confg
                          builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
                          
                      });
                });

        /// <summary>
        /// 根据环境变量定向配置文件名称
        /// </summary>
        private static string GetAppSettingsConfigName()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "")
            {
                return $".{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.";
            }
            else
            {
                return ".";
            }
        }
    }
}