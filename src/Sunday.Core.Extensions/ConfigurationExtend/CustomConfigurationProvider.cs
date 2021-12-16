using Microsoft.Extensions.Configuration;
using System;

namespace Sunday.Core.Extensions.ConfigurationExtend
{
    /// <summary>
    /// 核心数据提供程序，核心就是依靠内部的字典Data
    /// </summary>
    public class CustomConfigurationProvider : ConfigurationProvider
    {
        private readonly CustomConfigurationOption _customConfigurationOption = null;
        public CustomConfigurationProvider(CustomConfigurationOption customConfigurationOption)
        {
            _customConfigurationOption = customConfigurationOption;
        }

        /// <summary>
        /// 启动加载数据即可
        /// </summary>
        public override void Load()
        {
            Console.WriteLine($"CustomConfigurationProvider load data");
            //当然也可以从数据库读取，然后放入Data中
            //var result = this._customConfigurationOption.DataInitFunc.Invoke();

            base.Data.Add("TodayCustom", "0625-Custom");
            base.Data.Add("RabbitMQOptions-Custom:HostName", "192.168.3.254-Custom");
            base.Data.Add("RabbitMQOptions-Custom:UserName", "guest-Custom");
            base.Data.Add("RabbitMQOptions-Custom:Password", "guest-Custom");
        }

        /// <summary>
        /// 其他的用的默认
        /// </summary>
        public override void Set(string key, string value)
        {
            base.Data.Add(key, value);
            //_customConfigurationOption.DataChangeAction(key, value);
        }
    }
}
