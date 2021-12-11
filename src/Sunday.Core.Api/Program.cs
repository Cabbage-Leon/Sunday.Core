using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sunday.Core.Extensions.Apollo;
using Microsoft.Extensions.Logging;
using System;

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
                               .AddJsonFile($"appsettings{ GetAppSettingsConfigName() }json", optional: true, reloadOnChange: false)
                               ;
                         //����Apollo��������
                         config.AddConfigurationApollo("appsettings.apollo.json");
                     })
                      .UseUrls("http://*:8081")
                      .ConfigureLogging((hostingContext, builder) =>
                      {
                          // 1.���˵�ϵͳĬ�ϵ�һЩ��־
                          builder.AddFilter("System", LogLevel.Error);
                          builder.AddFilter("Microsoft", LogLevel.Error);

                          // 2.Ҳ������appsettings.json�����ã�LogLevel�ڵ�

                          // 3.ͳһ����
                          builder.SetMinimumLevel(LogLevel.Error);

                          // Ĭ��log4net.confg
                          builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
                      });
                });

        /// <summary>
        /// ���ݻ����������������ļ�����
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