import AuthApi from "../../api/user/AuthApi";
import React from "react";
import UserApi from "../../api/user/UserApi";
import {NoteUserBlank} from "../../models/blank/user/NoteUserBlank";

export default class Account extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            user : new NoteUserBlank()
        }
    }

    async componentDidMount() {
        let user = await UserApi.getUser();
        
        this.setState({user: user})
    }
    
    async out() {
        await AuthApi.signOut()
        window.location.replace("http://localhost:3000/")
    }

    render() {
        return (
            <div>
                
                <label>Вы вошли как: {this.state.user.email}</label>
                
                <button className="btn btn-primary-submit"
                                style={{margin: "5px"}}
                        onClick={this.out}>Exit</button>

            </div>
        )
    }

}
