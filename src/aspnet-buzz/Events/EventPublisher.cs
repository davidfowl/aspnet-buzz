using System;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using Microsoft.Framework.Logging;
using Jabbot.ApiHelpers.Github;

namespace AspNet.Buzz
{
    public class EventPublisher
    {
        private readonly IHubContext<EventsHub> _stream;
        
        private readonly ILogger<EventPublisher> _logger;

        private readonly IEventStore<ApiEvent> _eventStore;
        
        public EventPublisher(IHubContext<EventsHub> stream,
                              ILogger<EventPublisher> logger,
                              IEventStore<ApiEvent> eventStore,
                              GithubEventHandler githubEventHandler)
        {
            _stream = stream;
            _logger = logger;
            _eventStore = eventStore;

            githubEventHandler.ApiEvent = OnEvent;
        }

        private void OnEvent(ApiEvent @event)
        {
            _eventStore.Add(@event);
            
            var serializedEvent = JsonConvert.SerializeObject(@event);

            _logger.LogInformation(serializedEvent);

            _stream.Clients.All.githubEvent(@event);
        }
    }
}