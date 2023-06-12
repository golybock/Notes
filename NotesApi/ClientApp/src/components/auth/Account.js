import AuthApi from "../../api/user/AuthApi";
import React from "react";

export default class Account extends React.Component {

    async out() {
        await AuthApi.signOut()
        window.location.replace("http://localhost:3000/")
    }

    render() {
        return (
            <div>

                <label>Account page</label>
                <button className="btn btn-primary-submit form-control" onClick={this.out}>Выйти
                </button>

            </div>
        )
    }

}
