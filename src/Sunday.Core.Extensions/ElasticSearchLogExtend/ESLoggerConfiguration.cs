using Microsoft.Extensions.Logging;
using System;

namespace Sunday.Core.Extensions.ElasticSearchLogExtend
{
    public class ESLoggerConfiguration
    {
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 索引名
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public FilterLoggerSettings Filter{ get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public void Init(string message) { }
    }
}
