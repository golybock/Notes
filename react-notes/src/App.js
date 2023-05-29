import './App.css';
import React from "react";
import Navigation from "./components/navigation/Navigation";
import Views from "./Views";
import AuthApi from "./api/user/AuthApi";
import SignIn from "./components/Auth/SignIn";

export default class App extends React.Component {


    componentDidMount() {
        this.setState({isNotAuth: AuthApi.token() == null})
    }

    constructor(props) {
        super(props);
        this.state = {
            isNotAuth: true
        }
    }

    render() {
        return (
            <div>
                {!this.state.isNotAuth && <Navigation/>}
                <Views/>
                {this.state.isNotAuth && <SignIn onClose={async () => {
                    this.setState({isNotAuth: false});
                }}/>}
            </div>
        );
    }
}
