/// <reference path="../../typings/react/react.d.ts"/>
/// <reference path="../../node_modules/typed-react/typed-react.d.ts"/>

import React = require("react");
import TypedReact = require("typed-react");

interface AspNetBuzzHeaderIProps {}
interface AspNetBuzzHeaderIState {
    title: string;
    subTitle: string;
}

class AspNetBuzzHeaderClass extends TypedReact.Component<AspNetBuzzHeaderIProps, AspNetBuzzHeaderIState> {
    constructor() {
        super();
    }

    getInitialState() {
        return {
            title: "ASP.NET 5 Live event feed",
	    subTitle: " The only place to get ASP.NET 5 updates!"
        };
    }

    componentDidMount() {
    }

    public render() {
        return React.DOM.header({id: "header", className: "header"},
            React.DOM.div({className: "container"},
                React.DOM.a(
                    {
                        className: "header-logo-invertocat",
                        href: "http://aspnetbuzz.com",
                        title: this.state.title
                    },
                    React.DOM.h2(
                        {
                            href: "/",
                            style:
                            {
                                fontWeight: "500",
                                margin: "0", lineHeight: "1.1"
                            }
                        },
                        this.state.title,
                        React.DOM.small(
                            {style: {fontSize: "65%", fontWeight: "400", lineHeight: "1", color: "#777"}},
                            this.state.subTitle
                        )
                    )
                )
            )
        );
    }
}

export var AspNetBuzzHeader = TypedReact.createClass(AspNetBuzzHeaderClass);