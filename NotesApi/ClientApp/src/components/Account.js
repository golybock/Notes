import AuthApi from "../api/user/AuthApi";
import React from "react";
import {GoogleLogout} from "react-google-login";

export default class Account extends React.Component {

    client_id = "989073554490-lhcf948sulr8o8n5u85ivnbbluh3ve51.apps.googleusercontent.com"

    out() {
        AuthApi.deleteTokens()
        window.location.replace("http://localhost:3000/")
    }

    onLogoutSuccess = () =>{

    }

    render() {
        return (
            <div>

                <label>Account page</label>

                <GoogleLogout clientId={this.client_id}
                              buttonText={"Logout"}
                onLogoutSuccess={this.onLogoutSuccess}/>

                <button onClick={this.out}>Выйти
                </button>

            </div>
        )
    }

}
