using System;
using System.Linq;
using Jabbot.ApiHelpers.Github;
using Microsoft.Framework.Logging;

namespace AspNet.Buzz
{
    public class GithubEventHandler
    {
        public GithubEventHandler()
        {
            IssueOpened = (repo, user, issueUrl) => { };
            IssueReopened = (repo, user, issueUrl) => { };
            IssueClosed = (repo, user, issueUrl) => { };
            IssueComment = (repo, user, issueTitle, commentUrl) => { };
            Pushed = (repo, user, branch, branchCommitsUrl, branchCommitMessages) => { };
            BranchCreated = (repo, user, branch, branchCommitsUrl) => { };
            BranchDeleted = (repo, user, branch) => { };
            PullRequestOpened = (repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl) => { };
            PullRequestClosed = (repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl) => { };
            PullRequestComment = (repo, owner, commenter, pullRequestId, pullRequestUrl, commentUrl, title) => { };
            CommitComment = (repo, user, commitCommentUrl) => { };
        }

        // Organization/Repo, User, IssueUrl
        public Action<string, string, string> IssueOpened;
        // Organization/Repo, User, IssueUrl
        public Action<string, string, string> IssueReopened;
        // Organization/Repo, User, IssueUrl
        public Action<string, string, string> IssueClosed;
        // Organization/Repo, User, IssueTitle, CommentUrl
        public Action<string, string, string, string> IssueComment;
        // Organization/Repo, User, Branch, BranchCommitsUrl, BranchCommitMessages
        public Action<string, string, string, string, string[]> Pushed;
        // Organization/Repo, User, Branch, BranchCommitsUrl
        public Action<string, string, string, string> BranchCreated;
        // Organization/Repo, User, Branch
        public Action<string, string, string> BranchDeleted;
        // Organization/Repo, User, Head, Base, PullRequestId, PullRequestUrl
        public Action<string, string, string, string, long, string> PullRequestOpened;
        // Organization/Repo, User, Head, Base, PullRequestId, PullRequestUrl
        public Action<string, string, string, string, long, string> PullRequestClosed;
        // Organization/Repo, PullRequestOwner, Commenter, PullRequestId, PullRequestUrl, CommentUrl, PullRequestTitle
        public Action<string, string, string, long, string, string, string> PullRequestComment;
        // Organization/Repo, User, CommitCommentUrl
        public Action<string, string, string> CommitComment;

        public void Handle(ILogger logger, ApiEvent e)
        {
            logger.LogInformation("Got a new event from Github: {eventType} -> {action}", e.Type, e?.Payload?.Action);

            string branch, branchCommitsUrl;
            switch (e.Type)
            {
                case "IssuesEvent":
                    if (e.Payload.Action == "closed")
                    {
                        IssueClosed(e.Repo.Name, e.Actor.Login, e.Payload.Issue.Html_Url);
                    }
                    else if (e.Payload.Action == "opened")
                    {
                        IssueOpened(e.Repo.Name, e.Actor.Login, e.Payload.Issue.Html_Url);
                    }
                    else if (e.Payload.Action == "reopened")
                    {
                        IssueReopened(e.Repo.Name, e.Actor.Login, e.Payload.Issue.Html_Url);
                    }
                    break;
                case "IssueCommentEvent":
                    if (e.Payload.Action == "created")
                    {
                        // Not a pull request comment
                        if (e.Payload.Pull_Request == null || String.IsNullOrEmpty(e.Payload.Pull_Request.Html_Url))
                        {
                            IssueComment(e.Repo.Name, e.Actor.Login, e.Payload.Issue.Title, e.Payload.Comment.Html_Url);
                        }
                        else
                        {
                            PullRequestComment(e.Repo.Name, e.Payload.Issue.User.Login, e.Actor.Login, e.Payload.Number, e.Payload.Pull_Request.Html_Url, e.Payload.Comment.Html_Url, e.Payload.Issue.Title);
                        }
                    }
                    break;
                case "PushEvent":
                    branch = e.Payload.Ref.Replace("refs/heads/", "");
                    branchCommitsUrl = "https://github.com/" + e.Repo.Name + "/commits/" + branch;
                    string[] branchCommitMessages = new string[0];

                    if (e.Payload.Commits != null)
                    {
                        branchCommitMessages = e.Payload.Commits.Select(c => c.Message).ToArray();
                    }

                    Pushed(e.Repo.Name, e.Actor.Login, branch, branchCommitsUrl, branchCommitMessages);
                    break;
                case "CreateEvent":
                    if (e.Payload.Ref_Type == "branch")
                    {
                        branch = e.Payload.Ref;
                        branchCommitsUrl = "https://github.com/" + e.Repo.Name + "/commits/" + branch;
                        BranchCreated(e.Repo.Name, e.Actor.Login, branch, branchCommitsUrl);
                    }
                    break;
                case "DeleteEvent":
                    if (e.Payload.Ref_Type == "branch")
                    {
                        branch = e.Payload.Ref;
                        BranchDeleted(e.Repo.Name, e.Actor.Login, branch);
                    }
                    break;
                case "PullRequestEvent":
                    if (e.Payload.Action == "opened")
                    {
                        PullRequestOpened(e.Repo.Name, e.Actor.Login, e.Payload.Pull_Request.Head.Ref, e.Payload.Pull_Request.Base.Ref, e.Payload.Number, e.Payload.Pull_Request.Html_Url);
                    }
                    else if (e.Payload.Action == "closed")
                    {
                        PullRequestClosed(e.Repo.Name, e.Actor.Login, e.Payload.Pull_Request.Head.Ref, e.Payload.Pull_Request.Base.Ref, e.Payload.Number, e.Payload.Pull_Request.Html_Url);
                    }

                    break;
                case "PullRequestReviewCommentEvent":
                    if (e.Payload.Issue.User == null)
                    {
                        PullRequestComment(e.Repo.Name, String.Empty, e.Actor.Login, e.Payload.Issue.Number, e.Payload.Comment.Html_Url.Substring(0, e.Payload.Comment.Html_Url.IndexOf("#")), e.Payload.Comment.Html_Url, e.Payload.Issue.Title);
                    }
                    else
                    {
                        PullRequestComment(e.Repo.Name, e.Payload.Issue.User.Login, e.Actor.Login, e.Payload.Issue.Number, e.Payload.Comment.Html_Url.Substring(0, e.Payload.Comment.Html_Url.IndexOf("#")), e.Payload.Comment.Html_Url, e.Payload.Issue.Title);
                    }
                    break;
                case "CommitCommentEvent":
                    CommitComment(e.Repo.Name, e.Actor.Login, e.Payload.Comment.Html_Url);
                    break;
            }
        }
    }
}
