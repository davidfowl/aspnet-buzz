using System;
using System.Linq;
using Jabbot.ApiHelpers.Github;
using Microsoft.Framework.Logging;

namespace AspNet.Buzz
{
    public class GithubEventHandler
    {
        public GithubEventHandler()
        {
            ApiEvent = (@event) => { };
        }

        // Organization/Repo, User, IssueUrl
        public Action<ApiEvent> ApiEvent;
        
        public void Handle(ILogger logger, ApiEvent e)
        {
            logger.LogInformation("Got a new event from Github: {eventType} -> {action}", e.Type, e?.Payload?.Action);

            ApiEvent(e);
        }
    }
}
