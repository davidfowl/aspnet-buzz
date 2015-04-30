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
            events: []
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