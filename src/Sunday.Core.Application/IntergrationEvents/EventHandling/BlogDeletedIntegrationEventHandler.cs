using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sunday.Core.EventBus;
using Sunday.Core.Infrastructure;

namespace Sunday.Core.Application.IntergrationEvents
{
    public class BlogDeletedIntegrationEventHandler : IIntegrationEventHandler<BlogDeletedIntegrationEvent>
    {
        private readonly ILogger<BlogDeletedIntegrationEventHandler> _logger;

        public BlogDeletedIntegrationEventHandler(ILogger<BlogDeletedIntegrationEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(BlogDeletedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Blog.Core", @event);

            ConsoleHelper.WriteSuccessLine($"----- Handling integration event: {@event.Id} at Blog.Core - ({@event})");

            await Task.Delay(100);
            //await _blogArticleServices.DeleteById(@event.BlogId.ToString());
        }
    }
}