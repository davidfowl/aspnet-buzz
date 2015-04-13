using System;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using Microsoft.Framework.Logging;
using Jabbot.ApiHelpers.Github;

namespace AspNet.Buzz
{
    public class EventPublishing
    {
        private IHubContext<EventsHub> _stream;
        private ILogger<EventPublishing> _logger;

        public EventPublishing(IHubContext<EventsHub> stream, 
                               ILogger<EventPublishing> logger,
                               GithubEventHandler githubEventHandler)
        {
            _stream = stream;
            _logger = logger;

            githubEventHandler.ApiEvent = OnEvent;
        }

        private void OnEvent(ApiEvent @event)
        {
            var serializedEvent = JsonConvert.SerializeObject(@event);

            _logger.LogInformation(serializedEvent);

            _stream.Clients.All.githubEvent(@event);
        }
    }
}