import React from "react";
import SignUp from "./SignUp";
import SignIn from "./SignIn";

interface IProps {
    onClose: Function
}

interface IState {
    registration: boolean;
    login: boolean;
}

export default class Auth extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);
        
        this.state = {
            registration: false,
            login: true
        }
    }

    setRegistration() {
        this.setState({login: false})
        this.setState({registration: true})
    }

    setLogin() {
        this.setState({login: true})
        this.setState({registration: false})
    }

    render() {
        return (
            <div className="auth">

                {/*login*/}
                {this.state.login &&
                    <SignIn onClose={this.props.onClose}
                            onRegistration={() => this.setRegistration()}/>}

                {/*registration*/}
                {this.state.registration &&
                    <SignUp onClose={this.props.onClose}
                            onLogin={() => this.setLogin()}/>}

            </div>
        )
    }

}