﻿/*using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using JabbR.Client;
using JabbR.Client.Models;
using JabbR.Models;
using Jabbot.ApiHelpers.Github;
using Jabbot.Commands;
using Jabbot.MessageHandlers.Github.Providers;

namespace Jabbot.MessageHandlers
{
    public class GithubHandler : IImportableMessageHandler
    {
        private string[] _activators = new string[] { "github", "gh" };
        private MessageManager _messageManager;
        private GithubEventUpdater _updater;
        private GithubEventHandler _handler;
        private IJabbRClient _client;
        private Configuration _configuration;
        private GithubApi _api;
        private bool _enabled;

        [ImportingConstructor]
        public GithubHandler(Configuration configuration, IJabbRClient client)
        {
            _enabled = false;
            _client = client;
            _configuration = configuration;

            // Add defaults
            OnPullRequestOpened += (repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl) => { };
            OnPullRequestClosed += (repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl) => { };

            if (_configuration.GithubMonitorEnabled)
            {
                Initialize();
            }
        }

        public event Action<string, string, string, string, long, string> OnPullRequestOpened;
        public event Action<string, string, string, string, long, string> OnPullRequestClosed;

        public void Initialize()
        {
            _handler = new GithubEventHandler();
            _messageManager = new MessageManager(new List<string>(_activators));
            _messageManager.AddHandler(new IssueProvider(_configuration, _client));

            if (_configuration.GithubConfiguration.MonitorUserEvents)
            {
                _api = new GithubApi(_configuration.GithubConfiguration.ApiServer, _configuration.GithubConfiguration.UserName, _configuration.GithubConfiguration.AccessToken);
            }
            else
            {
                _api = new GithubApi(_configuration.GithubConfiguration.ApiServer, _configuration.GithubConfiguration.AccessToken, _configuration.GithubConfiguration.Organization, _configuration.GithubConfiguration.Repo);
            }

            BindHandler(_client, _configuration.DefaultChannel);

            _updater = new GithubEventUpdater(_configuration, _handler, _api);
            _enabled = true;
        }

        private void PostNotification(string message, string room)
        {
            var notification = new ClientNotification
            {
                Content = message,
                ImageUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyRpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoTWFjaW50b3NoKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpFNTE3OEEyQTk5QTAxMUUyOUExNUJDMTA0NkE4OTA0RCIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDpFNTE3OEEyQjk5QTAxMUUyOUExNUJDMTA0NkE4OTA0RCI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOkU1MTc4QTI4OTlBMDExRTI5QTE1QkMxMDQ2QTg5MDREIiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOkU1MTc4QTI5OTlBMDExRTI5QTE1QkMxMDQ2QTg5MDREIi8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+m4QGuQAAAyRJREFUeNrEl21ojWEYx895TDPbMNlBK46IUiNmPvHBSUjaqc0H8pF5+aDUKPEBqU2NhRQpX5Rv5jWlDIWlMCv7MMSWsWwmb3tpXub4XXWdPHvc9/Gc41nu+nedc7/8r/99PffLdYdDPsvkwsgkTBwsA/PADJCnzX2gHTwBt8Hl7p537/3whn04XoDZDcpBlk+9P8AFcAghzRkJwPF4zGGw0Y9QS0mAM2AnQj77FqCzrtcwB1Hk81SYojHK4DyGuQ6mhIIrBWB9Xm7ug/6B/nZrBHBegrkFxoVGpnwBMSLR9EcEcC4qb8pP14BWcBcUgewMnF3T34VqhWMFkThLJAalwnENOAKiHpJq1FZgI2AT6HZtuxZwR9GidSHtI30jOrbawxlVX78/AbNfhHlomEUJJI89O2MqeE79T8/nk8nMBm/dK576hZgmA3cp/R4l9/UeSxiHLVIlNm4nFfT0bxyuIj7LHRTKai+zdJobwMKzcZSJb0ePV5PKN+BqAAKE47UlMnERELMM3EdYP/yrd+XYb2mOiYBiQ8OQnoRBlXrl9JZix7D1pHTazu4MoyBcnYamqAjIMTR8G4FT8LuhLsexXYYjICBiqhQBvYb6fLZIJCjPypVvaOoVAW2WcasCnL2Nq82xHJNSqlCeFcDshaPK0twkAhosjZL31QYw+1rlMpWGMArl23SBsZZO58F2tlJXmjOXS+s4WGvpMiBJT/I2PInZ6lIs9/hBsNS1hS6BG0DSqmYEDRlCXQrmy50P1oDRKTSegmNbUsA0zDMwRhPJXeCE3vWLPQMvan6X8AgIa1vcR4AkGZkDR4ejJ1UHpsaVI0g2LInpOsNFUud1rhxSV+fzC9Woz2EZkWQuja7/B+jUrgtIMpy9YCW4n4K41YfzRneW5E1KJTe4B2Zq1Q5EHEtj4U3AfEzR5SVY4l7QYQPJdN2as7RKBF0BPZqqH4VgMAMBL8Byxr7y8zCZiDlnOcEKIPmUpgB5Z2ww5RdOiiRiNajUmWda5IG6WbhsyY2fx6m8gLcoJDJFkH219M3We1+cnda93pfycZpIJEL/s/wSYADmOAwAQgdpBAAAAABJRU5ErkJggg==",
                Room = room,
                Source = "Github"
            };

            _client.PostNotification(notification);
        }

        private void BindHandler(IJabbRClient client, string defaultRoom)
        {
            _handler.IssueOpened = (repo, user, issueUrl) =>
            {
                PostNotification(String.Format("|{0}| => {1} has **opened** issue {2}.", repo, user, issueUrl), defaultRoom);
            };

            _handler.IssueReopened = (repo, user, issueUrl) =>
            {
                PostNotification(String.Format("|{0}| => {1} has **reopened** issue {2}.", repo, user, issueUrl), defaultRoom);
            };

            _handler.IssueClosed = (repo, user, issueUrl) =>
            {
                PostNotification(String.Format("|{0}| => {1} has **closed** issue {2}.", repo, user, issueUrl), defaultRoom);
            };

            _handler.IssueComment = (repo, user, issueTitle, commentUrl) =>
            {
                PostNotification(String.Format("|{0}| => {1} has commented on issue '{2}' ({3}).", repo, user, issueTitle, commentUrl), defaultRoom);
            };

            _handler.Pushed = (repo, user, branch, branchCommitsUrl, branchCommitMessages) =>
            {
                if (branchCommitMessages.Length > 1)
                {
                    PostNotification(String.Format("|{0}| [{1}] => {2} has has pushed {3} commits.  ({4})", repo, branch, user, branchCommitMessages.Length, branchCommitsUrl), defaultRoom);
                }
                else if(branchCommitMessages.Length == 1)
                {
                    PostNotification(String.Format("|{0}| [{1}] => {2} has has pushed commit '{3}'.  ({4})", repo, branch, user, branchCommitMessages[0], branchCommitsUrl), defaultRoom);
                }
                else
                {
                    PostNotification(String.Format("|{0}| [{1}] => {2} has has pushed.  ({3})", repo, branch, user, branchCommitsUrl), defaultRoom);
                }
            };

            _handler.BranchCreated = (repo, user, branch, branchCommitsUrl) =>
            {
                PostNotification(String.Format("|{0}| => {1} has created branch '{2}'.  ({3}).", repo, user, branch, branchCommitsUrl), defaultRoom);
            };

            _handler.BranchDeleted = (repo, user, branch) =>
            {
                PostNotification(String.Format("|{0}| => {1} has deleted branch '{2}'.", repo, user, branch), defaultRoom);
            };

            _handler.PullRequestOpened = (repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl) =>
            {
                PostNotification(String.Format("|{0}| => {1} has opened Pull Request #{2} from '{3}' -> '{4}'.  ({5})", repo, user, pullRequestId, branchHead, branchBase, pullRequestUrl), defaultRoom);

                OnPullRequestOpened(repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl);
            };

            _handler.PullRequestClosed = (repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl) =>
            {
                PostNotification(String.Format("|{0}| => {1} has closed Pull Request #{2} from '{3}' -> '{4}'.  ({5})", repo, user, pullRequestId, branchHead, branchBase, pullRequestUrl), defaultRoom);

                OnPullRequestClosed(repo, user, branchHead, branchBase, pullRequestId, pullRequestUrl);
            };

            _handler.PullRequestComment = (repo, owner, commenter, pullRequestId, pullRequestUrl, commentUrl, title) =>
            {
                if (String.IsNullOrEmpty(owner))
                {
                    PostNotification(String.Format("|{0}| => {1} has commented on pull request #{2} '{3}' with comment {4}.", repo, commenter, pullRequestId, title, commentUrl), defaultRoom);
                }
                else
                {
                    PostNotification(String.Format("|{0}| => {1} has commented on {2}'s pull request #{3} '{4}' with comment {5}.", repo, commenter, owner, pullRequestId, title, commentUrl), defaultRoom);

                }
            };

            _handler.CommitComment = (repo, user, commitCommentUrl) =>
            {
                PostNotification(String.Format("|{0}| => {1} has commented on commit {2}.", repo, user, commitCommentUrl), defaultRoom);
            };
        }

        public void Handle(BotCommand command)
        {
            if (_enabled)
            {
                if (_activators.Contains(command.Command))
                {
                    var message = new Message
                    {
                        Content = command.Command + command.Argument,
                        User = command.OriginalMessage.User,
                        When = command.OriginalMessage.When,
                        Id = command.OriginalMessage.Id
                    };

                    _messageManager.Handle(message, command.Room);
                }
            }
        }
    }
}
*/