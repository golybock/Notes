import React from "react";
import "./Auth.css"
import sakura from '/src/sakura.jpg'
import AuthApi from "../../api/user/AuthApi";

export default class Registration extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            email: "",
            password: "",
            name: "",
            error: ""
        }
    }

    auth = async () => {

        let r = await AuthApi.signUp(this.state.email, this.state.password)

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
                                        <label>Name</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            placeholder="Enter name"
                                            value={this.state.name}
                                            onChange={(e) => {
                                                this.setState({
                                                    name: e.target.value
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
                                        <button className="btn btn-primary-submit" onClick={this.auth}>
                                            Sign Up
                                        </button>
                                    </div>
                                </form>
                                <div className="mt-3">
                                    <p className="mb-0 text-muted">Already have an account?</p>
                                    <div className="btn btn-primary" onClick={this.props.onLogin}>Log in
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
