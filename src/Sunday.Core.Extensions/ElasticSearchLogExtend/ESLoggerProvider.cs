using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sunday.Core.Extensions.ElasticSearchLogExtend
{
    public class ESLoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ESClientProvider _esClient;
        private readonly string _indexName;
        private readonly FilterLoggerSettings _filter;

        public ESLoggerProvider(ESLoggerConfiguration configuration)
        {
            _httpContextAccessor = configuration.ServiceProvider.GetService<IHttpContextAccessor>();
            _indexName = configuration.IndexName;

            _esClient = configuration.ServiceProvider.GetService<ESClientProvider>();
            _esClient.EnsureIndexWithMapping<LogEntry>(_indexName);

            _filter = configuration.Filter ?? new FilterLoggerSettings
            {
                {"*", LogLevel.Warning}
            };
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ESLogger(_esClient, _httpContextAccessor, categoryName, FindLevel(categoryName));
        }

        private LogLevel FindLevel(string categoryName)
        {
            var def = LogLevel.Warning;
            foreach (var s in _filter.Switches)
            {
                if (categoryName.Contains(s.Key))
                {
                    return s.Value;
                }

                if (s.Key == "*")
                {
                    def = s.Value;
                }
            }

            return def;
        }

        public void Dispose()
        {
            // Nothing to do
        }
    }
}
