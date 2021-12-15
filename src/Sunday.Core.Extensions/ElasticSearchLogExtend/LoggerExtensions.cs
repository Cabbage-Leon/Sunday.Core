using Microsoft.Extensions.Logging;
using System;

namespace Sunday.Core.Extensions.ElasticSearchLogExtend
{
    public static class LoggerExtensions
    {
        public static ILoggerFactory AddESLogger(this ILoggerFactory factory, ESLoggerConfiguration configuration)
        {
            factory.AddProvider(new ESLoggerProvider(configuration));
            return factory;
        }

        /// <summary>
        /// 传递Action(option委托初始化，套路的一种)
        /// 可以赋值，也可以执行方法
        /// </summary>
        public static ILoggerFactory AddCustomConsoleLogger(this ILoggerFactory loggerFactory, Action<ESLoggerConfiguration> configure)
        {
            var config = new ESLoggerConfiguration();

            configure.Invoke(config);

            return loggerFactory.AddESLogger(config);
        }
    }
}
