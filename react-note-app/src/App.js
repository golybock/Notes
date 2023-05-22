import logo from './logo.svg';
import './App.css';
import React from "react";
import tagApi from "./api/note/tag/TagApi";

class MainApp extends React.Component {

    async componentDidMount() {
        this.setState({tags: await tagApi.getTags()})
        console.log(this.state.tags.toString())
    }

    constructor(props) {
        super(props);
        this.state = {
            tags: []
        };
    }

    render() {
        return (
            <div className="App">
                <header className="App-header">
                    <h1>{this.state.tags.toString()}</h1>
                    <img src={logo} className="App-logo" alt="logo"/>
                    <p>
                        Edit <code>src/App.js</code> and save to reload.
                    </p>
                    <a
                        className="App-link"
                        href="https://reactjs.org"
                        target="_blank"
                        rel="noopener noreferrer"
                    >
                        Learn React
                    </a>
                </header>
            </div>
        );
    }
}

export default MainApp;
