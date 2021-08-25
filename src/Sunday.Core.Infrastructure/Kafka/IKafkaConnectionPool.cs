using Confluent.Kafka;
using System;

namespace Sunday.Core.Infrastructure
{
    /// <summary>
    /// Kafka连接池
    /// </summary>
    public interface IKafkaConnectionPool : IDisposable
    {
        /// <summary>
        /// 取对象
        /// </summary>
        /// <returns></returns>
        IProducer<string, byte[]> Producer();

        /// <summary>
        /// 将对象放入连接池
        /// </summary>
        bool Return(IProducer<string, byte[]> producer);
    }
}