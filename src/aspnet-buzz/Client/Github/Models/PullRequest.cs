
namespace Jabbot.ApiHelpers.Github
{
    public class PullRequest
    {
        public long Number { get; set; }
        public string Html_Url { get; set; } 
        public string Title { get; set; }
        public Head Head { get; set; }
        public Base Base { get; set; }
        public long Commits { get; set; }
        public long Additions { get; set; }
        public long Deletions { get; set; }
    }
}
