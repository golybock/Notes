import AuthApi from "../../api/user/AuthApi";
import React from "react";
import UserApi from "../../api/user/UserApi";
import IUserView from "../../models/view/user/IUserView";

interface IProps {

}

interface IState {
    user?: IUserView
}

export default class Account extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);
        this.state = {
            user: undefined
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
                
                {/*if user not undefinded*/}
                {this.state.user && (
                    <div>
                        <label>Вы вошли как: {this.state.user.email}</label>

                        <button className="btn btn-primary-submit"
                                style={{margin: "5px"}}
                                onClick={this.out}>Exit
                        </button>

                    </div>
                )}
                
            </div>
        )
    }

}
