using System;
using System.Collections.Concurrent;
using System.Threading;
using Jabbot.ApiHelpers.Github;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Logging;

namespace AspNet.Buzz
{
    public class GithubEventUpdater
    {
        public static readonly TimeSpan PollPeriod = TimeSpan.FromSeconds(5); // Get updates every 5 seconds
        private int _gettingEvents;
        private Timer _eventUpdater;
        private GithubApi _api;
        private GithubEventHandler _eventHandler;
        private ConcurrentDictionary<long, ApiEvent> _eventCache;
        private ILogger<GithubEventUpdater> _logger;

        public GithubEventUpdater(ILogger<GithubEventUpdater> logger, GithubEventHandler handler, IOptions<GithubOptions> optionsAccessor)
        {
            _logger = logger;
            var options = optionsAccessor.Options;
            
            logger.LogDebug("S={server} U={user} AT={accessToken}", options.GithubApiServer, options.GithubUsername, options.GithubAccessToken);

            _api = new GithubApi(options.GithubApiServer, options.GithubAccessToken, options.GithubOrganization, "");
            _gettingEvents = 0;
            _eventHandler = handler;
            _eventCache = new ConcurrentDictionary<long, ApiEvent>();

            Initialize();
        }

        public async void Initialize()
        {
            var events = await _api.GetEvents();

            foreach (ApiEvent e in events)
            {
                _eventCache.TryAdd(e.ID, e);
            }

            _eventUpdater = new Timer(PollGitHubEvents, null, PollPeriod, PollPeriod);
        }


        private async void PollGitHubEvents(object state)
        {
            if (Interlocked.Exchange(ref _gettingEvents, 1) == 1)
            {
                return;
            }

            var events = await _api.GetEvents();

            try
            {
                foreach (ApiEvent e in events)
                {
                    // If the event id exists in the cache then its an old entry
                    if (_eventCache.Keys.Contains(e.ID))
                    {
                        continue;
                    }

                    _eventCache.TryAdd(e.ID, e);

                    if (e.Type == "PullRequestReviewCommentEvent")
                    {
                        var pullRequestData = await _api.GetPullRequestData(e.Payload.Comment.Pull_Request_Url);
                        e.Payload.Issue = pullRequestData;
                    }

                    _eventHandler.Handle(_logger, e);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                Interlocked.Exchange(ref _gettingEvents, 0);
            }
        }
    }
}
