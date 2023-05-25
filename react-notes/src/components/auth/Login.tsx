import React from "react";

export class Login extends React.Component{

    render() {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-5 mx-auto">
                        <div className="card card-body">

                            <form id="submitForm" action="/login" method="post" data-parsley-validate="" data-parsley-errors-messages-disabled="true">
                                <div className="form-group required">
                                    <label>Username / Email</label>
                                    <input type="text" className="form-control text-lowercase" id="username" required name="username" value=""></input>
                                </div>
                                <div className="form-group required">
                                    <label className="d-flex flex-row align-items-center">Password
                                        <a className="ml-auto border-link small-xl" href="/forget-password">Forget?</a></label>
                                    <input type="password" className="form-control" required id="password" name="password" value=""></input>
                                </div>
                                <div className="form-group pt-1">
                                    <button className="btn btn-primary btn-block" type="submit">Log In</button>
                                </div>
                            </form>
                            <p className="small-xl pt-3 text-center">
                                <span className="text-muted">Not a member?</span>
                                <a href="/signup">Sign up</a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
