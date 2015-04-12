using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jabbot.ApiHelpers.AspNetForum
{
    public class AspNetForumApi
    {
        private string _server;
        
        public AspNetForumApi(string server)
        {
            _server = server;
        }

        public async Task<AspNetForumResponse> GetQuestionsSince(long since)
        {
            Stream stream;
            using (var client = new HttpClient())
            {
                var httpResponse = await client.GetAsync(_server);
                stream = await httpResponse.Content.ReadAsStreamAsync();
            }
            var xml = XDocument.Load(stream);

            var response = new AspNetForumResponse();
            var items = (from c in xml.Descendants("item")
                         where ParsePubDate(c.Element("pubDate").Value) > since
                         select new Question
                         {
                             Title = (string)c.Element("title"),
                             Link = (string)c.Element("link"),
                             Creation_Date = ParsePubDate(c.Element("pubDate").Value),
                         }).ToList<Question>();
            response.Questions = items;
            return response;
        }

        private static long ParsePubDate(string date)
        {
            return DateTime.Parse(date).Ticks;
        }
    }
}
