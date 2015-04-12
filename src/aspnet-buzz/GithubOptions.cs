using System;

namespace AspNet.Buzz
{
    public class GithubOptions
    {
        public string GithubApiServer { get; set; } = "https://api.github.com";
        public string GithubUsername { get; set; }
        public string GithubAccessToken { get; set; }
        public string GithubOrganization { get; set; }
        public string GithubRepo { get; set; }
        public bool GithubMonitorUserEvents { get; set; }
    }
}
