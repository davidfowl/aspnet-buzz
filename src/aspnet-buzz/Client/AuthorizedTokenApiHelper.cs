using System.Net;
using RestSharp;

namespace Jabbot.ApiHelpers
{
    public class AuthorizedTokenApiHelper : ApiHelper
    {
        protected string _token;

        public AuthorizedTokenApiHelper(RestClient restClient, string token)
            : base(restClient)
        {
            _token = token;
        }

        protected virtual RestRequest BuildAuthorizedRestRequest(string url)
        {
            var request = new RestRequest(url);
            request.AddParameter("access_token", _token);

            return request;
        }

        protected virtual WebRequest BuildAuthorizedWebRequest(string url, string method = "GET")
        {
            var request = WebRequest.Create(string.Format("{0}/{1}?access_token={2}", RestClient.BaseUrl, url, _token));

            request.Method = method;

            return request;
        }
    }
}
