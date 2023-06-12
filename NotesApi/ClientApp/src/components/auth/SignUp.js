import React from "react";
import "./Auth.css"
import sakura from "./../../resources/sakura.jpg"
import AuthApi from "../../api/user/AuthApi";
import {NoteUserBlank} from "../../models/blank/user/NoteUserBlank";

export default class SignUp extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            email: "",
            password: "",
            error: ""
        }
    }

    signUp = async () => {

        let user = new NoteUserBlank(this.state.email, this.state.password);
        
        let r = await AuthApi.signUp(user)

        if (r === true) {
            this.props.onClose()
        } else {
            this.setState({error: "Неверный логин или пароль"});
        }
    }
    render() {
        return (
            <div className="container">
                <div className="body d-md-flex align-items-center justify-content-between">
                    <div className="box-1 mt-md-0 mt-5">
                        <img
                            src={sakura}
                            alt={sakura}/>
                    </div>
                    <div className=" box-2 d-flex flex-column h-100">
                        <div className="mt-5">
                            <p className="mb-1 h-1">Create Account.</p>
                            <div className="d-flex flex-column ">
                                <form onSubmit={() =>{

                                }}>
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
                                        <button className="btn btn-primary-submit" onClick={this.signUp}>
                                            Sign Up
                                        </button>
                                    </div>
                                </form>
                                <div className="mt-3">
                                    <p className="mb-0 text-muted">Already have an account?</p>
                                    <div className="hint-button" onClick={this.props.onLogin}>Log in
                                        <span className="fas fa-chevron-right ms-1"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>)
    }
}

