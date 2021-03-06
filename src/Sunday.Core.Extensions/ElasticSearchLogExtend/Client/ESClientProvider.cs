using Microsoft.Extensions.Options;
using Nest;
using System;

namespace Sunday.Core.Extensions.ElasticSearchLogExtend
{
    public class ESClientProvider
    {
        public ESClientProvider(IOptions<ESOptions> options)
        {
            var settings = new ConnectionSettings(new Uri(options.Value.Uri))
                .DefaultIndex(options.Value.DefaultIndex);

            if (!String.IsNullOrEmpty(options.Value.UserName) && !String.IsNullOrEmpty(options.Value.Password))
            {
                settings.BasicAuthentication(options.Value.UserName, options.Value.Password);
            }

            this.Client = new ElasticClient(settings);
            this.DefaultIndex = options.Value.DefaultIndex;
            //EnsureIndexWithMapping<LogModel>(this.DefaultIndex);
        }

        public ElasticClient Client { get; private set; }
        public string DefaultIndex { get; private set; }

        public void EnsureIndexWithMapping<T>(string indexName = null, Func<PutMappingDescriptor<T>, PutMappingDescriptor<T>> customMapping = null) where T : class
        {
            if (String.IsNullOrEmpty(indexName)) indexName = this.DefaultIndex;

            // Map type T to that index
            this.Client.ConnectionSettings.DefaultIndices.Add(typeof(T), indexName);

            // Does the index exists?
            var indexExistsResponse = this.Client.Indices.Exists(new IndexExistsRequest(indexName));
            if (!indexExistsResponse.IsValid) throw new InvalidOperationException(indexExistsResponse.DebugInformation);

            // If exists, return
            if (indexExistsResponse.Exists) return;

            // Otherwise create the index and the type mapping
            var createIndexRes = this.Client.Indices.Create(indexName);
            if (!createIndexRes.IsValid) throw new InvalidOperationException(createIndexRes.DebugInformation);

            var res = this.Client.Map<T>(m =>
            {
                m.AutoMap().Index(indexName);
                if (customMapping != null) m = customMapping(m);
                return m;
            });

            if (!res.IsValid) throw new InvalidOperationException(res.DebugInformation);
        }
    }
    
}
