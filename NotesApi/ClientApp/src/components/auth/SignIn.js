import React from "react";
import "./Auth.css"
import sakura from "./../../resources/sakura.jpg"
import AuthApi from "../../api/user/AuthApi";

export default class SignIn extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            email: "Aboba123!@gmail.com",
            password: "Aboba123!@gmail.com",
            error: ""
        }
    }

    signIn = async () => {

        let r = await AuthApi.signIn(this.state.email, this.state.password)

        if (r === true) {
            this.props.onClose()
        } else {
            this.setState({error: "Неверный логин или пароль"});
        }
    }

    render() {
        return (<div>

            <div className="container">
                <div className="body d-md-flex align-items-center justify-content-between">
                    <div className="box-1 mt-md-0 mt-5">
                        <img
                            src={sakura}
                            alt={sakura}/>
                    </div>
                    <div className=" box-2 d-flex flex-column h-100">
                        <div className="mt-5">
                            <p className="mb-1 h-1">Sign in.</p>
                            <div className="d-flex flex-column ">
                                <div className="mb-3">
                                    <label>Email address</label>
                                    <input
                                        type="email"
                                        className="form-control"
                                        placeholder="Enter email"
                                        value={this.state.email}
                                        onChange={(e) => {
                                            this.setState({
                                                email: e.target.value
                                            })
                                        }}
                                    />
                                </div>
                                <div className="mb-3">
                                    <label>Password</label>
                                    <input
                                        type="password"
                                        className="form-control"
                                        placeholder="Enter password"
                                        value={this.state.password}
                                        onChange={(e) => {
                                            this.setState({
                                                password: e.target.value
                                            })
                                        }}
                                    />
                                </div>
                                <div className="d-grid">
                                    <button onClick={this.signIn}
                                            className="btn btn-primary-submit">
                                        Sign in
                                    </button>
                                </div>
                                <div className="mt-3">
                                    <p className="mb-0 text-muted">Dont have an account?</p>
                                    <button className="hint-button" onClick={this.props.onRegistration}>Registration
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>)

    }
}