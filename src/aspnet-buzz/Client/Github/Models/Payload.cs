
using System.Collections.Generic;
namespace Jabbot.ApiHelpers.Github
{
    public class Payload
    {
        public string Action { get; set; }
        public long Number { get; set; }
        public Issue Issue { get; set; }
        public Comment Comment { get; set; }
        public List<Commit> Commits { get; set; }
        public PullRequest Pull_Request { get; set; }
        public string Ref { get; set; }
        public string Ref_Type { get; set; }
    }
}
