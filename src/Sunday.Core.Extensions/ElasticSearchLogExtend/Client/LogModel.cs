using Nest;
using System;
using System.Linq;

namespace Sunday.Core.Extensions.ElasticSearchLogExtend
{
    [ElasticsearchType(RelationName = "Log")]
    public class LogModel
    {
        public string Appid { set; get; }

        public string Level { set; get; }

        public object Tags { set; get; }

        public DateTime Time { set; get; }

        public string Title { set; get; }

        public string IP { set; get; }

        public string Message { set; get; }

        public long Ticks { get; set; }

        public string GetIndexTypeName()
        {
            string indexTypeName = null;
            if (GetType().GetCustomAttributes(typeof(ElasticsearchTypeAttribute), true).FirstOrDefault() is ElasticsearchTypeAttribute att)
                indexTypeName = att.RelationName;

            if (string.IsNullOrWhiteSpace(indexTypeName))
                indexTypeName = GetType().Name;

            return indexTypeName;
        }
    }
}
