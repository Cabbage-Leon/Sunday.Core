using Microsoft.Extensions.Configuration;
using System;

namespace Sunday.Core.Extensions.ConfigurationExtend
{
    /// <summary>
    /// 数据源：负责创建ConfigurationProvider--数据提供程序
    /// </summary>
    public class CustomConfigurationSource : IConfigurationSource
    {
        private readonly Action<CustomConfigurationOption> _optionsAction;

        public CustomConfigurationSource(Action<CustomConfigurationOption> optionsAction)
        {
            _optionsAction = optionsAction;
        }

        /// <summary>
        /// 创建provider
        /// </summary>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            CustomConfigurationOption customConfigurationOption = new CustomConfigurationOption();
            this._optionsAction.Invoke(customConfigurationOption);

            return new CustomConfigurationProvider(customConfigurationOption);
        }
    }
}
