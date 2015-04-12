
namespace Jabbot.ApiHelpers.Github
{
    public class Issue
    {
        public string Html_Url { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public long Number { get; set; }
    }
}
