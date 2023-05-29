import AuthApi from "../api/user/AuthApi";
import React from "react";

export default class Account extends React.Component{

  render() {
    return(
        <div>

          <label>Account page</label>

          <button onClick={() => {
            AuthApi.deleteTokens()
          }}>Выйти</button>

        </div>
    )
  }

}
