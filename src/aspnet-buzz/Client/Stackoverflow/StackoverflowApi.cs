using System.Threading.Tasks;
using RestSharp;

namespace Jabbot.ApiHelpers.Stackoverflow
{
    public class StackoverflowApi : ApiHelper
    {
        public StackoverflowApi(string server)
            : base(new RestClient(server))
        {
        }

        public Task<StackoverflowResponse> GetQuestionsSince(string tag, long since)
        {
            var questionRequest = new RestRequest("2.2/questions");
            questionRequest.AddParameter("answers", false);
            questionRequest.AddParameter("body", false);
            questionRequest.AddParameter("comments", false);
            questionRequest.AddParameter("sort", "creation");
            questionRequest.AddParameter("tagged", tag);
            questionRequest.AddParameter("fromdate", since);
            questionRequest.AddParameter("site", "stackoverflow");

            return Execute<StackoverflowResponse>(questionRequest);
        }
    }
}
