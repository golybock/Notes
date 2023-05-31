import AuthApi from "../api/user/AuthApi";
import React from "react";

export default class Account extends React.Component {

    out() {
        AuthApi.deleteTokens()
        window.location.replace("http://localhost:3000/")
    }

    render() {
        return (
            <div>

                <label>Account page</label>

                <button onClick={this.out}>Выйти
                </button>

            </div>
        )
    }

}
