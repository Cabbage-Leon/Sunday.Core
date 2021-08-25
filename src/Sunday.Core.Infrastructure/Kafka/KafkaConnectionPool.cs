using System.Collections.Concurrent;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Sunday.Core.Model.Options;

namespace Sunday.Core.Infrastructure
{
    /// <summary>
    /// Kafka producer 连接池管理
    /// 可以使用微软官方的对象池进行构造ObjectPool
    /// </summary>
    public class KafkaConnectionPool : IKafkaConnectionPool
    {
        private readonly KafkaOptions _options;
        private readonly ConcurrentQueue<IProducer<string, byte[]>> _producerPool;
        private int _currentCount;
        private int _maxSize;

        public KafkaConnectionPool(IOptions<KafkaOptions> options)
        {
            _options = options.Value;
            _maxSize = _options.ConnectionPoolSize;
        }

        /// <summary>
        /// 取对象
        /// </summary>
        public IProducer<string, byte[]> Producer()
        {
            if (_producerPool.TryDequeue(out var producer))
            {
                Interlocked.Decrement(ref _currentCount);
                return producer;
            }

            var config = new ProducerConfig()
            {
                BootstrapServers = _options.Servers,
                QueueBufferingMaxMessages = 10,
                MessageTimeoutMs = 5000,
                RequestTimeoutMs = 3000
            };

            producer = new ProducerBuilder<string, byte[]>(config).Build();
            return producer;
        }

        /// <summary>
        /// 将对象放入连接池
        /// </summary>
        public bool Return(IProducer<string, byte[]> producer)
        {
            if (Interlocked.Increment(ref _currentCount) <= _maxSize)
            {
                _producerPool.Enqueue(producer);
                return true;
            }

            producer.Dispose();
            Interlocked.Decrement(ref _currentCount);

            return false;
        }

        public void Dispose()
        {
            _maxSize = 0;
            _currentCount = 0;
            while (_producerPool.TryDequeue(out var context))
            {
                context?.Dispose();
            }
        }
    }
}