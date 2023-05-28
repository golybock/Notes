import React from "react";
import "./Auth.css"
import sakura from '/src/sakura.jpg'
// import close from '/src/close.svg'
import AuthApi from "../../api/user/AuthApi";


class Registration extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            email: "aboba12@aboba.com",
            password: "Password1!"
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
                            {/*<p className="text-muted mb-2">Share your thouhts with the world form today.</p>*/}
                            <div className="d-flex flex-column ">
                                {/*<p className="text-muted mb-2">Continue with...</p>*/}
                                {/*<div className="d-flex align-items-center">*/}
                                {/*    <a href="#" className="box me-2 selectio">*/}
                                {/*        <span className="fab fa-facebook-f mb-2"></span>*/}
                                {/*        <p className="mb-0">Facebook</p>*/}
                                {/*    </a>*/}
                                {/*    <a href="#" className="box me-2">*/}
                                {/*        <span className="fab fa-google mb-2"></span>*/}
                                {/*        <p className="mb-0">Google</p>*/}
                                {/*    </a>*/}
                                {/*    <a href="#" className="box">*/}
                                {/*        <span className="far fa-envelope mb-2"></span>*/}
                                {/*        <p className="mb-0">Email</p>*/}
                                {/*    </a>*/}
                                {/*</div>*/}
                                <form onSubmit={() =>{

                                }}>
                                    <div className="mb-3">
                                        <label>Email address</label>
                                        <input
                                            type="email"
                                            className="form-control"
                                            placeholder="Enter email"
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label>Name</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            placeholder="Enter name"
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label>Password</label>
                                        <input
                                            type="password"
                                            className="form-control"
                                            placeholder="Enter password"
                                        />
                                    </div>
                                    <div className="d-grid">
                                        <button type="submit" className="btn btn-primary-submit">
                                            Sign Up
                                        </button>
                                    </div>
                                </form>
                                <div className="mt-3">
                                    <p className="mb-0 text-muted">Already have an account?</p>
                                    <div className="btn btn-primary">Log in
                                        <span className="fas fa-chevron-right ms-1"></span>
                                    </div>
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

export default Registration;
