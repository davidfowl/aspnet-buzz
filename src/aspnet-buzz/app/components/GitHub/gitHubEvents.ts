/// <reference path="../../../typings/react/react.d.ts"/>
/// <reference path="../../../node_modules/typed-react/typed-react.d.ts"/>
/// <reference path="../../../typings/marked/marked.d.ts"/>

import React = require("react");
import TypedReact = require("typed-react");
import Models = require("./GitHubModels");
import Marked = require("marked");

interface GitHubEventIProps { model: Models.GitHubModels.ApiEvent }
interface GitHubEventIState { }

class ViewModel {
    user: string;
    avatar_url: string;
    octicon: string;
    message: string;
    title: any[];
    repoUrl: string;
    repoName: string;
    branch: string;
    commits: any[];
    commitUrl: string;
    action: string;
}

class GitHubEventClass extends TypedReact.Component<GitHubEventIProps, GitHubEventIState> {
    
    render() {
        return React.DOM.div(
            {className: "body"},
            [getGitHubEventTemplate(this.props)]
        )
    }
}

function IssueTemplate(model: ViewModel): React.DOMElement<any>[] {
    return [
        React.DOM.span(
            {className: "mega-octicon octicon-" + model.octicon },
            ""
        ),
        React.DOM.div(
            {className: "title"},
            React.DOM.a(
                {href: "https://github.com/" + model.user, target: "_blank"},
                model.user
            ),
            " ",
            model.title
        ),
        React.DOM.div(
            {className: "details"},
            getAvatar(model.user, model.avatar_url),
            React.DOM.div(
                {className: "message markdown-body"},
                React.DOM.blockquote(
                    {dangerouslySetInnerHTML: {__html: model.message}},
                    null
                )
            )
        )
    ]
}

function PushTemplate(model: ViewModel): React.DOMElement<any>[] {
    return [
        React.DOM.span(
            {className: "mega-octicon octicon-" + model.octicon},
            null
        ),
        React.DOM.div(
            {className: "title"},
            React.DOM.a(
                {href: "https://github.com/" + model.user, target: "_blank"},
                model.user
            ),
            " ",
            model.title
        ),
        React.DOM.div(
            {className: "details"},
            getAvatar(model.user, model.avatar_url),
            React.DOM.div(
                {className: "commits pusher-is-only-committer"},
                React.DOM.ul(
                    null,
                    model.commits
                )
            )
        )
    ];
}

function ActionTemplate(model: ViewModel): React.DOMElement<any>[]  {
    return [
        React.DOM.div(
            {className: "simple"},
            React.DOM.span(
                {className: "octicon octicon-" + model.octicon},
                null
            ),
            React.DOM.div(
                {className: "title"},
                React.DOM.a(
                    {href: "https://github.com/" + model.user, target: "_blank"},
                    model.user
                ),
                " ",
                model.title
            )
        )
    ];
}

function getAvatar(user: string, avatar_url: string): React.DOMElement<any> {
    return React.DOM.a(
        {href: "https://github.com/" + user, target: "_blank"},
        React.DOM.img(
            {
                alt: "@" + user,
                className: "gravatar",
                height: 30,
                width: 30,
                src: avatar_url
            },
            ""
        )
    )
}
        
function getOcticon(event: Models.GitHubModels.ApiEvent): string {
    switch(event.Type) {
    case "IssuesEvent":
    case "PullRequestEvent":
        if(event.Payload.Pull_Request) {
            return "git-pull-request";
        }
        else if(event.Payload.Action == "opened") {
            return "issue-opened";
        }
        else if(event.Payload.Action == "reopened") {
            return "issue-reopened";
        }
        else if(event.Payload.Action == "closed") {
            return "issue-closed";
        }
        break;
    case "IssueCommentEvent":
    case "PullRequestReviewCommentEvent":
        return "comment-discussion";
    case "PushEvent":
        return "git-commit";
    case "WatchEvent":
        return "star";
    case "CreateEvent":
    case "DeleteEvent":
    case "ForkEvent":
        return "git-branch";
    }
}

function getTitlePrefix(event: Models.GitHubModels.ApiEvent): React.DOMElement<any> {
    var obj = event.Payload.Issue || event.Payload.Pull_Request;
    var htmlUrl = event.Payload.Comment ? event.Payload.Comment.Html_Url : obj.Html_Url;

    return React.DOM.a(
        {
            href: htmlUrl,
            target: "_blank",
            title: obj.Title
        },
        event.Repo.Name + "#" + obj.Number
    );
}

function getBranchName(refs: string): string {
    return refs.replace("refs/heads/", "");
}

function getBranchUrl(repoName: string, branchName: string): string {
    return "https://github.com/" + repoName + "/tree/" + branchName;
}

function getBranchCommitUrl(repoName: string, branchName: string): string {
    return "https://github.com/" + repoName + "/commits/" + branchName;
}

function getForkedRepositoryName(userName: string, fullRepoName: string): string {
    var index = fullRepoName.indexOf("/");
    var repoName = fullRepoName.substr(index, fullRepoName.length - index);
    return userName + repoName;
}

function getRepositoryUrl(repoName: string): string {
    return "https://github.com/" + repoName;
}

function getRawMarkdown(markdown: string): string {
    return Marked(
        markdown,
        {
            sanitize: true,
            gfm: true,
            breaks: true,
            tables: true
        }
    );
}

function getCompareCommitsUrl(repoName: string, firstSha: string, lastSha: string): string {
    return "https://github.com/" + repoName + "/compare/" + firstSha + "..." + lastSha;
}

function getMoreCommitsMessage(count: number, repoName, firstSha: string, lastSha: string): React.DOMElement<any> {
    return React.DOM.li(
        {className: "more"},
        React.DOM.a(
            {
                href: getCompareCommitsUrl(repoName, firstSha, lastSha),
                target: "_blank"
            },
            count,
            " more ",
            count == 1 ? "commit" : "commits",
            " »"
        )
    );
}

function getCommit(user: Models.GitHubModels.Actor, commit: Models.GitHubModels.Commit): React.DOMElement<any> {
    return React.DOM.li(
        null,
        React.DOM.span(
            {title: user.Login},
            React.DOM.img(
                {
                    alt: user.Login,
                    height: "16",
                    width: "16",
                    src: user.Avatar_Url + "?v=3&amp;s=32"
                },
                null
            )
        ),
        " ",
        React.DOM.code(
            null,
            React.DOM.a(
                {
                    href: commit.Url,
                    target: "_blank"
                },
                commit.Sha.substr(0, 7)
            )
        ),
        " ",
        React.DOM.div(
            {className: "message"},
            React.DOM.blockquote(
                null,
                commit.Message
            )
        )
    );
}

function getCommits(event: Models.GitHubModels.ApiEvent): React.DOMElement<any>[] {
    var commits: React.DOMElement<any>[] = [];
    if(event.Payload.Commits.length <= 2) {
        for(var i = 0; i < event.Payload.Commits.length; i++) {
            commits.push(getCommit(event.Actor, event.Payload.Commits[i]));
        }
    }
    else {
        for(var i = 0; i < 2; i++) {
            commits.push(getCommit(event.Actor, event.Payload.Commits[i]));
            commits.push(getMoreCommitsMessage(event.Payload.Commits.length - 2, event.Repo.Name, event.Payload.Commits[0].Sha.substr(0, 7), event.Payload.Commits[event.Payload.Commits.length - 1].Sha.substr(0, 7)));
        }
    }

    return commits;
}

function getViewModel(event: Models.GitHubModels.ApiEvent): ViewModel {
    var vm = new ViewModel();
    vm.user = event.Actor.Login;
    vm.avatar_url = event.Actor.Avatar_Url;
    vm.repoName = event.Repo.Name;
    vm.repoUrl = getRepositoryUrl(vm.repoName);
    vm.octicon = getOcticon(event);

    if(event.Type == "IssuesEvent" || event.Type == "PullRequestEvent")
    {
        var obj = event.Payload.Issue || event.Payload.Pull_Request;
        vm.title = [
            React.DOM.span(
                null,
                event.Payload.Action
            ),
            " issue ",
            getTitlePrefix(event)
        ];
        vm.message = getRawMarkdown(obj.Title);
    }
    else if(event.Type == "IssueCommentEvent" || event.Type == "PullRequestReviewCommentEvent")
    {
        var obj = event.Payload.Issue || event.Payload.Pull_Request;
        if(event.Payload.Action == "created")
        {
            vm.title = [
                React.DOM.span(
                    null,
                    " commented"
                ),
                " on ",
                (event.Payload.Pull_Request ? "pull request " : "issue "),
                getTitlePrefix(event)
            ];
            vm.message = getRawMarkdown(event.Payload.Comment.Body || "");
            vm.octicon = getOcticon(event);
        }
    }
    else if(event.Type == "PushEvent") {
        vm.branch = getBranchName(event.Payload.Ref);
        vm.commits = getCommits(event);
        vm.commitUrl = getBranchCommitUrl(vm.repoName, vm.branch);
        vm.title = [
            React.DOM.span(
                null,
                " pushed"
            ),
            " to ",
            React.DOM.a(
                {
                    href: getBranchUrl(vm.repoName, vm.branch),
                    target: "_blank"
                },
                vm.branch
            ),
            " at ",
            React.DOM.a(
                {
                    href: vm.repoUrl,
                    target: "_blank"
                },
                vm.repoName
            )
        ];
    }
    else if(event.Type == "CreateEvent" || event.Type == "DeleteEvent") {
        if(event.Payload.Ref_Type == "branch")
        {
            vm.branch = event.Payload.Ref;
            vm.action = event.Type == "CreateEvent" ? "created" : "deleted";
            vm.title = [
                React.DOM.span(
                    null,
                    vm.action
                ),
                " branch ",
                React.DOM.a(
                    {
                        href: getBranchUrl(vm.repoName, vm.branch),
                        target: "_blank",
                        className: "css-truncate css-truncate-target branch-name",
                        title: vm.branch
                    },
                    vm.branch
                ),
                " at ",
                React.DOM.a(
                    {
                        href: vm.repoUrl,
                        target: "_blank"
                    },
                    vm.repoName
                )
            ];
        }
    }
    else if(event.Type == "WatchEvent") {
        if(event.Payload.Action == "started") {
            vm.action = "starred";
            vm.title = [
                React.DOM.span(
                    null,
                    vm.action
                ),
                " ",
                React.DOM.a(
                    {
                        href: getRepositoryUrl(vm.repoName),
                        target: "_blank"
                    },
                    vm.repoName
                )
            ];
        }
    }
    else if(event.Type == "ForkEvent") {
        var forkedRepoName = getForkedRepositoryName(vm.user, vm.repoName);
        vm.action = "forked";
        vm.title = [
            React.DOM.span(
                null,
                vm.action
            ),
            " ",
            React.DOM.a(
                {
                    href: vm.repoUrl,
                    target: "_blank"
                },
                vm.repoName
            ),
            " to ",
            React.DOM.a(
                {
                    href: getRepositoryUrl(forkedRepoName),
                    target: "_blank"
                },
                forkedRepoName
            )
        ];
    }
    
    console.log(vm);
    return vm;
}

function getGitHubEventTemplate(props: GitHubEventIProps): React.DOMElement<any>[] {
    switch(props.model.Type) {
    case "IssuesEvent":
    case "PullRequestEvent":
    case "IssueCommentEvent":
    case "PullRequestReviewCommentEvent":
        return IssueTemplate(getViewModel(props.model));
    case "PushEvent":
        return PushTemplate(getViewModel(props.model));
    case "CreateEvent":
    case "DeleteEvent":
    case "WatchEvent":
    case "ForkEvent":
        return ActionTemplate(getViewModel(props.model));
    default:
        console.log("Received a yet to be implemented event type: " + props.model.Type);
    }
}    

export var GitHubEvent = TypedReact.createClass(GitHubEventClass);