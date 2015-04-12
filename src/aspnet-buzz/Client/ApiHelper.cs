using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace Jabbot.ApiHelpers
{
    public class ApiHelper
    {
        public ApiHelper(RestClient restClient)
        {
            RestClient = restClient;
        }

        public RestClient RestClient { get; private set; }

        public Task<T> Execute<T>(RestRequest request) where T : new()
        {
            var task = new TaskCompletionSource<T>();

            RestClient.ExecuteAsync<T>(request, response =>
            {
                task.SetResult(response.Data);
            });

            return task.Task;
        }

        public Task Execute(RestRequest request)
        {
            var task = new TaskCompletionSource<object>();

            RestClient.ExecuteAsync(request, response =>
            {
                task.SetResult(null);
            });

            return task.Task;
        }

        public async Task<string> Retrieve(WebRequest request)
        {
            using(var responseStream = (await request.GetResponseAsync()).GetResponseStream())
            using (var streamReader = new StreamReader(responseStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        public async Task Push(WebRequest request, string value)
        {
            using(var requestStream = await request.GetRequestStreamAsync())
            using (var streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.WriteLine(value);
            }

            await request.GetResponseAsync();
        }
    }
}
