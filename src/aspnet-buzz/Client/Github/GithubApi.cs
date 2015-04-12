using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace Jabbot.ApiHelpers.Github
{
    public class GithubApi : AuthorizedTokenApiHelper
    {
        private string _requestUrl;
        private string _gitHubServer;

        public GithubApi(string server, string username, string token)
            : base(new RestClient(server), token)
        {
            _gitHubServer = server;

            _requestUrl = string.Format("/users/{0}/received_events", username);
        }

        public GithubApi(string server, string token, string organization, string repo)
            : base(new RestClient(server), token)
        {
            _gitHubServer = server;

            if (string.IsNullOrEmpty(repo))
            {
                _requestUrl = String.Format("orgs/{0}/events", organization);
            }
            else
            {
                _requestUrl = String.Format("repos/{0}/{1}/events", organization, repo);
            }
        }

        public Task<List<ApiEvent>> GetEvents()
        {
            return this.Execute<List<ApiEvent>>(BuildAuthorizedRestRequest(_requestUrl));
        }

        public Task<Issue> GetPullRequestData(string pullRequestUrl)
        {
            return this.Execute<Issue>(BuildAuthorizedRestRequest(pullRequestUrl.Replace(_gitHubServer, "")));
        }
    }
}
