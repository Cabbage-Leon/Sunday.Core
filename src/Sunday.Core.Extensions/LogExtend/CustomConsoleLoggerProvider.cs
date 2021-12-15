using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Sunday.Core.Extensions.LogExtend
{
    /// <summary>
    /// 驱动程序：为每个类都创建一个写日志的实例，保存在字典里面
    ///           其实也可以多种类型的，控制台、文本、等等
    /// </summary>
    public class CustomConsoleLoggerProvider : ILoggerProvider
    {
        private readonly CustomConsoleLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, CustomConsoleLogger> _loggersDictionary = new ConcurrentDictionary<string, CustomConsoleLogger>();

        public CustomConsoleLoggerProvider(CustomConsoleLoggerConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 为每个类都创建一个写日志的实例，保存在字典里面，其实也可以多种类型的，控制台、文本、等等
        /// </summary>
        public ILogger CreateLogger(string categoryName)
        {
            return _loggersDictionary.GetOrAdd(categoryName, name => new CustomConsoleLogger(name, _config));
        }

        public void Dispose()
        {
            _loggersDictionary.Clear();
        }
    }
}
