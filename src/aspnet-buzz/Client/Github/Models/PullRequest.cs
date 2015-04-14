
namespace Jabbot.ApiHelpers.Github
{
    public class PullRequest
    {
        public long Number { get; set; }
        public string Html_Url { get; set; } 
        public string Title { get; set; }
        public Head Head { get; set; }
        public Base Base { get; set; }
    }
}
