using Microsoft.Extensions.Logging;

namespace Sunday.Core.Extensions.LogExtend
{
    /// <summary>
    /// 接受外部的配置
    /// </summary>
    public class CustomConsoleLoggerConfiguration
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        
        /// <summary>
        /// Id
        /// </summary>
        public int EventId { get; set; } = 0;

        /// <summary>
        /// 方法
        /// </summary>
        public void Init(string message){ }
    }
}
