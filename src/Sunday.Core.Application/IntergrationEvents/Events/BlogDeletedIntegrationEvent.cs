using Sunday.Core.EventBus;

namespace Sunday.Core.Application.IntergrationEvents
{
    public class BlogDeletedIntegrationEvent : IntegrationEvent
    {
        public string BlogId { get; private set; }

        public BlogDeletedIntegrationEvent(string blogid) => BlogId = blogid;
    }
}