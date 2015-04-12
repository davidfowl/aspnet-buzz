using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Framework.Logging;


namespace AspNet.Buzz
{
    public class EventPublishing
    {
        private IHubContext<EventsHub> _stream;
        private ILogger<EventPublishing> _logger;

        public EventPublishing(IHubContext<EventsHub> stream, 
                               ILogger<EventPublishing> logger,
                               GithubEventHandler githubEventHandler)
        {
            _stream = stream;
            _logger = logger;

            githubEventHandler.IssueOpened = IssueOpened;
            githubEventHandler.IssueClosed = IssueClosed;
            githubEventHandler.IssueReopened = IssueReopened;
            githubEventHandler.IssueComment = IssueComment;
            githubEventHandler.Pushed = Pushed;
            githubEventHandler.PullRequestOpened = PullRequestOpened;
            githubEventHandler.PullRequestComment = PullRequestComment;
            githubEventHandler.CommitComment = CommitComment;
        }

        private void IssueOpened(string repo, string user, string issueUrl)
        {
            _stream.Clients.All.issueOpened(repo, user, issueUrl);
            _logger.LogInformation(String.Format("|{0}| => {1} has **opened** issue {2}.", repo, user, issueUrl));
        }

        private void IssueClosed(string repo, string user, string issueUrl)
        {
            _stream.Clients.All.issueClosed(repo, user, issueUrl);
             _logger.LogInformation(String.Format("|{0}| => {1} has **closed** issue {2}.", repo, user, issueUrl));
        }

        private void IssueReopened(string repo, string user, string issueUrl)
        {
            _stream.Clients.All.issueReopened(repo, user, issueUrl);
             _logger.LogInformation(String.Format("|{0}| => {1} has **reopened** issue {2}.", repo, user, issueUrl));
        }

        private void IssueComment(string repo, string user, string issueTitle, string commentUrl)
        {
            _stream.Clients.All.issueComment(repo, user, issueTitle, commentUrl);
             _logger.LogInformation(String.Format("|{0}| => {1} has commented on issue '{2}' ({3}).", repo, user, issueTitle, commentUrl));
        }

        private void Pushed(string repo, string user, string branch, string branchCommitsUrl, string[] branchCommitMessages)
        {
            _stream.Clients.All.pushed(repo, user, branch, branchCommitsUrl, branchCommitMessages);

            if (branchCommitMessages.Length > 1)
            {
                _logger.LogInformation(String.Format("|{0}| [{1}] => {2} has has pushed {3} commits.  ({4})", repo, branch, user, branchCommitMessages.Length, branchCommitsUrl));
            }
            else if(branchCommitMessages.Length == 1)
            {
                _logger.LogInformation(String.Format("|{0}| [{1}] => {2} has has pushed commit '{3}'.  ({4})", repo, branch, user, branchCommitMessages[0], branchCommitsUrl));
            }
            else
            {
                _logger.LogInformation(String.Format("|{0}| [{1}] => {2} has has pushed.  ({3})", repo, branch, user, branchCommitsUrl));
            }
        }

        private void PullRequestOpened(string repo, string user, string branchHead, string branchBase, long pullRequestId, string pullRequestUrl)
        {
            _stream.Clients.All.pullRequestOpened(repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl);

            _logger.LogInformation(String.Format("|{0}| => {1} has opened Pull Request #{2} from '{3}' -> '{4}'.  ({5})", repo, user, pullRequestId, branchHead, branchBase, pullRequestUrl));
        }

        private void PullRequestComment(string repo, string pullRequestOwner, string commenter, long pullRequestId, string pullRequestUrl, string commentUrl, string pullRequestTitle)
        {
            
        }

        private void CommitComment(string repo, string user, string commitCommentUrl)
        {
            
        }
    }
}