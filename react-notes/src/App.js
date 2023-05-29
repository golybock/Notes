import './App.css';
import React from "react";
import Navigation from "./components/navigation/Navigation";
import Views from "./Views";

export default class App extends React.Component {


    componentDidMount() {

    }

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <Navigation/>
                <Views/>
            </div>
        );
    }
}
