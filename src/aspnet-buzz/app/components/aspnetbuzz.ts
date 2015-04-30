/// <reference path="../../typings/react/react.d.ts"/>
/// <reference path="../../node_modules/typed-react/typed-react.d.ts"/>

import React = require("react");
import TypedReact = require("typed-react");
import component = require("./Header");
import streamComponent = require("./EventStream");

interface AspNetBuzzIProps { }
interface AspNetBuzzIState { }

class AspNetBuzzClass extends TypedReact.Component<AspNetBuzzIProps, AspNetBuzzIState> {
    constructor() {
      super();
    }

    getInitialState() {
        return {};
    }

    componentDidMount() {
    }

    public render() {
        return React.DOM.div(null,
            React.createElement(component.AspNetBuzzHeader),
            React.DOM.div({className: "container"},
                React.createElement(streamComponent.AspNetBuzzEventStream)
            )
        );
    }
};

export var AspNetBuzzApp = TypedReact.createClass(AspNetBuzzClass);
