using System;
using Microsoft.Extensions.Logging;

namespace Sunday.Core.Extensions.LogExtend
{
    /// <summary>
    /// 扩展个方法，让ILoggerFactory去AddProvider
    /// </summary>
    public static class CustomConsoleLoggerExtensions
    {
        /// <summary>
        /// 直接传递对象
        /// </summary>
        public static ILoggerFactory AddCustomConsoleLogger(this ILoggerFactory loggerFactory, CustomConsoleLoggerConfiguration config)
        {
            loggerFactory.AddProvider(new CustomConsoleLoggerProvider(config));
            return loggerFactory;
        }

        /// <summary>
        /// 默认对象
        /// </summary>
        public static ILoggerFactory AddCustomConsoleLogger(this ILoggerFactory loggerFactory)
        {
            var config = new CustomConsoleLoggerConfiguration();
            return loggerFactory.AddCustomConsoleLogger(config);
        }

        /// <summary>
        /// 传递Action(option委托初始化，套路的一种)
        /// 可以赋值，也可以执行方法
        /// </summary>
        public static ILoggerFactory AddCustomConsoleLogger(this ILoggerFactory loggerFactory, Action<CustomConsoleLoggerConfiguration> configure)
        {
            var config = new CustomConsoleLoggerConfiguration();

            configure.Invoke(config);

            return loggerFactory.AddCustomConsoleLogger(config);
        }
    }
}
