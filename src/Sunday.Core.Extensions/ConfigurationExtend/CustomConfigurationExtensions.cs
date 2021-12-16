using Microsoft.Extensions.Configuration;
using System;

namespace Sunday.Core.Extensions.ConfigurationExtend
{
    /// <summary>
    /// 做个扩展
    /// </summary>
    public static class CustomConfigurationExtensions
    {
        public static IConfigurationBuilder AddCustomConfiguration(this IConfigurationBuilder builder, Action<CustomConfigurationOption> optionsAction)
        {
            return builder.Add(new CustomConfigurationSource(optionsAction));//配置个数据源
        }
    }
}
