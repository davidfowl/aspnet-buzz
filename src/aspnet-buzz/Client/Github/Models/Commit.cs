
namespace Jabbot.ApiHelpers.Github
{
    public class Commit
    {
        public string Sha { get; set; }
        public string Message { get; set; }
        public bool Distinct { get; set; }
        public string Url { get; set; }
    }
}
