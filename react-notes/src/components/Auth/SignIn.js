import React from "react";
import "./Auth.css"
import sakura from '/src/sakura.jpg'
import AuthApi from "../../api/user/AuthApi";
import {useLocation, useNavigate} from "react-router";
import {Navigate} from "react-router-dom";

class SignIn extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            email : "aboba12@aboba.com",
            password : "Password1!"
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
                            <p className="mb-1 h-1">Sign in.</p>
                            <div className="d-flex flex-column ">
                                <form onSubmit={async () => {

                                    let r = await AuthApi.login(this.state.email, this.state.password)

                                    if (r === true) {
                                        alert("signed")

                                        const navigate = useNavigate();
                                        const location = useLocation();

                                        return <Navigate to="/home"/>
                                        // navigate("/home")
                                    }
                                    else{
                                        alert("invalid sign")
                                    }
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
                                        <button type="submit" className="btn btn-primary-submit">
                                            Sign in
                                        </button>
                                    </div>
                                </form>
                                <div className="mt-3">
                                    <p className="mb-0 text-muted">Dont have an account?</p>
                                    <button className="btn btn-primary" onClick={() => {
                                        const navigate = useNavigate();
                                        navigate("registration");
                                    }}>Registration
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    {/*<span className="fas fa-times">*/}
                    {/*    <img src={close} alt={close}/>*/}
                    {/*</span>*/}
                </div>
            </div>)
    }
}

export default SignIn;
