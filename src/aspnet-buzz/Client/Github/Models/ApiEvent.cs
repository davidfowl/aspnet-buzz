
namespace Jabbot.ApiHelpers.Github
{
    public class ApiEvent
    {
        public string Type { get; set; }
        public long ID { get; set; }
        public Repo Repo { get; set; }
        public Payload Payload { get; set; }
        public Actor Actor { get; set; }
    }
}
