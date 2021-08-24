using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Sunday.Core.Infrastructure
{
    public class Appsettings
    {
        private static IConfiguration Configuration { get; set; } = null;

        public Appsettings(string contentPath, string envName)
        {
            Configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{envName}.json", true, true)
               .Build();
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        public static string App(params string[] sections)
        {
            if (sections.Any())
            {
                return Configuration[string.Join(":", sections)];
            }
            return string.Empty;
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        public static List<T> App<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }
    }
}