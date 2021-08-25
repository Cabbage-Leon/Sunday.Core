using InitQ.Abstractions;
using InitQ.Attributes;
using Sunday.Core.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Sunday.Core.Application.RedisMQ
{
    public class RedisSubscribe : IRedisSubscribe
    {
        //订阅Redis队列的key
        [Subscribe(RedisMqKey.Loging)]
        private async Task SubRedisLoging(string msg)
        {
            Console.WriteLine($"订阅者 2 从 队列{RedisMqKey.Loging} 消费到/接受到 消息:{msg}");

            await Task.CompletedTask;
        }
    }
}