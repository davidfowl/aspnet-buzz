using System;
using System.Net;
using System.Text;
using RestSharp;

namespace Jabbot.ApiHelpers
{
    public class AuthorizedBasicApiHelper : ApiHelper
    {
        protected string _username;
        protected string _password;

        private string _encodedAuth;        

        public AuthorizedBasicApiHelper(RestClient restClient, string username, string password)
            : base(restClient)
        {
            _username = username;
            _password = password;
            _encodedAuth = "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(username + ":" + password));
        }

        public RestRequest BuildAuthorizedRestRequest(string url)
        {
            var request = new RestRequest(url);
            request.AddHeader("Authorization", _encodedAuth);

            return request;
        }

        public WebRequest BuildAuthorizedWebRequest(string url, string method)
        {
            var request = WebRequest.Create(RestClient.BaseUrl + "/" + url);

            request.Method = method;

            request.Headers.Add("Authorization", _encodedAuth);

            return request;
        }
    }
}
