/// <reference path="../../typings/react/react.d.ts"/>
/// <reference path="../../node_modules/typed-react/typed-react.d.ts"/>
/// <reference path="../../typings/jquery/jquery.d.ts"/>
/// <reference path="../../typings/signalr/signalr.d.ts"/>

import React = require("react");
import TypedReact = require("typed-react");
import Models = require("./GitHub/GitHubModels");
import GitHub = require("./GitHub/GitHubEvents");
var supportedEvents: string[] = [
    "IssuesEvent",
    "PullRequestEvent",
    "IssueCommentEvent",
    "PullRequestReviewCommentEvent",
    "PushEvent",
    "CreateEvent",
    "DeleteEvent",
    "WatchEvent",
    "ForkEvent"
];

var issuesEvent = {"Type":"IssuesEvent","ID":2736226780,"Repo":{"Name":"aspnet/dnvm"},"Payload":{"Action":"closed","Number":0,"Issue":{"Html_Url":"https://github.com/aspnet/dnvm/issues/218","Title":"Beta4 DNVM points to the myget aspnetrelease feed instead of nuget.org","User":{"Login":"muratg","Avatar_Url":"https://avatars.githubusercontent.com/u/1808428?v=3"},"Number":218},"Comment":null,"Commits":null,"Pull_Request":null,"Ref":null,"Ref_Type":null},"Actor":{"Login":"muratg","Avatar_Url":"https://avatars.githubusercontent.com/u/1808428?"}};

var pullRequestEvent = {"Type":"PullRequestEvent","ID":2740070032,"Repo":{"Name":"aspnet/EntityFramework","Url":"https://api.github.com/repos/aspnet/EntityFramework","Html_Url":null},"Payload":{"Action":"opened","Number":2035,"Issue":null,"Comment":null,"Commits":null,"Pull_Request":{"Number":2035,"Html_Url":"https://github.com/aspnet/EntityFramework/pull/2035","Title":"Allow generic GetFieldValue to be used in query","Head":{"Ref":"BunniesDo414"},"Base":{"Ref":"dev"},"Commits":1,"Additions":279,"Deletions":270},"Ref":null,"Ref_Type":null},"Actor":{"Login":"ajcvickers","Avatar_Url":"https://avatars.githubusercontent.com/u/1430078?"}};

var issueCommentEvent = {"Type":"IssueCommentEvent","ID":2736170530,"Repo":{"Name":"aspnet/Session"},"Payload":{"Action":"created","Number":0,"Issue":{"Html_Url":"https://github.com/aspnet/Session/issues/20","Title":"Session Events","User":{"Login":"hishamco","Avatar_Url":"https://avatars.githubusercontent.com/u/3237266?v=3"},"Number":20},"Comment":{"Pull_Request_Url":null,"Html_Url":"https://github.com/aspnet/Session/issues/20#issuecomment-94240808","Body":"@Tratcher thanks for your clarification, nothing but checking the validity of the data session after ```await next()``` is a headache specially if you have some data stored in the session, perhaps some pages change some of them those data, so providing such events will let the web devs focusing on their business logic rather than writing a tedious code that may fails in some unexpected situations"},"Commits":null,"Pull_Request":null,"Ref":null,"Ref_Type":null},"Actor":{"Login":"hishamco","Avatar_Url":"https://avatars.githubusercontent.com/u/3237266?"}};

var commitCommentEvent = {"Type":"CommitCommentEvent","ID":2740888335,"Repo":{"Name":"aspnet/dnx","Url":"https://api.github.com/repos/aspnet/dnx","Html_Url":null},"Payload":{"Action":null,"Number":0,"Issue":null,"Comment":{"Pull_Request_Url":null,"Html_Url":"https://github.com/aspnet/dnx/commit/15ceff04e9c49d382d9668728041ed5cfda69f6e#commitcomment-10818338","Body":"Great"},"Commits":null,"Pull_Request":null,"Ref":null,"Ref_Type":null},"Actor":{"Login":"davidfowl","Avatar_Url":"https://avatars.githubusercontent.com/u/95136?"}};

var pushEvent = {"Type":"PushEvent","ID":2736207631,"Repo":{"Name":"aspnet/SignalR-Client-Cpp"},"Payload":{"Action":null,"Number":0,"Issue":null,"Comment":null,"Commits":[{"Sha":"efd3714647a14d663e578aa5afd1cf8b475873fc","Message":"Zip! - adding private symbols alongside NuGet package","Distinct":false,"Url":"https://api.github.com/repos/aspnet/SignalR-Client-Cpp/commits/efd3714647a14d663e578aa5afd1cf8b475873fc"},{"Sha":"71d9d005eb0025cdb5c1685ccc9631f673a541f2","Message":"Merge branch 'release' into dev","Distinct":true,"Url":"https://api.github.com/repos/aspnet/SignalR-Client-Cpp/commits/71d9d005eb0025cdb5c1685ccc9631f673a541f2"}],"Pull_Request":null,"Ref":"refs/heads/dev","Ref_Type":null},"Actor":{"Login":"moozzyk","Avatar_Url":"https://avatars.githubusercontent.com/u/1438884?"}};

var watchEvent = {"Type":"WatchEvent","ID":2736180928,"Repo":{"Name":"aspnet/EntityFramework"},"Payload":{"Action":"started","Number":0,"Issue":null,"Comment":null,"Commits":null,"Pull_Request":null,"Ref":null,"Ref_Type":null},"Actor":{"Login":"hungnd1475","Avatar_Url":"https://avatars.githubusercontent.com/u/10368418?"}};

var forkEvent = {"Type":"ForkEvent","ID":2736187005,"Repo":{"Name":"aspnet/aspnet-docker"},"Payload":{"Action":null,"Number":0,"Issue":null,"Comment":null,"Commits":null,"Pull_Request":null,"Ref":null,"Ref_Type":null},"Actor":{"Login":"northtyphoon","Avatar_Url":"https://avatars.githubusercontent.com/u/2686301?"}};

interface AspNetBuzzEventStreamIProps { }
interface AspNetBuzzEventStreamIState {
    events: Models.GitHubModels.ApiEvent[];
}

class AspNetBuzzEventStreamClass extends TypedReact.Component<AspNetBuzzEventStreamIProps, AspNetBuzzEventStreamIState> {
    constructor() {
        super();
    }

    getInitialState() {
        return {
            events: [/*
                issuesEvent,
                pullRequestEvent,
                issueCommentEvent,
                commitCommentEvent,
                pushEvent,
                watchEvent,
                forkEvent
            */]
        };
    }

    initialize() {
        var connection = $.hubConnection();
        connection.logging = true;
        var hub = connection.createHubProxy('events');

        hub.on('githubEvent', (e) => {
            console.log(e);
            var newEvents = [e].concat(this.state.events);
            this.setState({events: newEvents});
        });

        connection.disconnected(() => {
            connection.hub.log("Dropped the connection from the server. Restarting in 5 seconds.", true);
            setTimeout(() => {
                this.initialize();
            }, 5000);
        });

        connection.start();
    }

    componentDidMount() {
        this.initialize();
    }

    getEventClassName(event: Models.GitHubModels.ApiEvent): string {
        var className = "alert";
        if(event.Type == "PushEvent") {
            className += " push";
        }
        else if(event.Type == "CreateEvent" || event.Type == "DeleteEvent") {
            className += " create simple";
        }
        return className;
    }

    isEventSupported(eventType: string): boolean {
        return supportedEvents.indexOf(eventType) >= 0;
    }
    
    public render() {
        var items = this.state.events.map((item) => {
            if(this.isEventSupported(item.Type)) {
                return React.DOM.div(
                    {key: item.ID, className: this.getEventClassName(item)},
                    React.createElement(GitHub.GitHubEvent, {model: item}, null)
                );
            }
        });
        return React.DOM.div({id: "stream", className: "news"}, items);
    }
}

export var AspNetBuzzEventStream = TypedReact.createClass(AspNetBuzzEventStreamClass);