/// <reference path="../typings/react/react.d.ts"/>
/// <reference path="../node_modules/typed-react/typed-react.d.ts"/>

import React = require("react");
import TypedReact = require("typed-react");
import component = require("./components/aspnetbuzz");

interface AppIProps {}
interface AppIState {}

class AppClass extends TypedReact.Component<AppIProps, AppIState> {
    render() {
          return React.createElement(component.AspNetBuzzApp, null, null);
    }
}

export var App = React.createFactory(TypedReact.createClass<AppIProps, AppIState>(AppClass));

function render() {
    var app = App({});
    
    React.render(app, document.getElementById("aspNetBuzzApp"));
}

render();