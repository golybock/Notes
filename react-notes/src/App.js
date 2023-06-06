import './App.css';
import React from "react";
import AuthApi from "./api/user/AuthApi";
import SignIn from "./components/Auth/SignIn";
import {Link, Route, Routes} from "react-router-dom";
import ProtectedRoutes from "./components/navigation/ProtectedRoutes";
import Home from "./components/home/Home";
import Account from "./components/Account";
import Cat from "./cat.webp";
import {gapi} from "gapi-script";

export default class App extends React.Component {

    // client_id = "989073554490-lhcf948sulr8o8n5u85ivnbbluh3ve51.apps.googleusercontent.com"

    constructor(props) {
        super(props);
        this.state = {
            isAuth: false,
            token : gapi.auth().getToken().access_token,
            start : function () {
                gapi.client.init({
                    clientId : "989073554490-lhcf948sulr8o8n5u85ivnbbluh3ve51.apps.googleusercontent.com",
                    scope: ""
                })

                gapi.load('client:auth2', this.start)
            }
        }
    }

    componentDidMount() {
        this.setState({isAuth: AuthApi.token() != null})
    }

    auth() {
        this.setState({isAuth: true});
    }

    render() {
        return (
            <div className="App">
                {/*nav bar*/}
                {this.state.isAuth && (
                    <nav>
                        <div className="Nav-panel">
                            <Link className="Navbar-item" to="/">
                                <img src={Cat} alt={Cat} className="App-logo"/>
                            </Link>
                            <Link className="Navbar-item" to="/">Главная</Link>
                            <Link className="Navbar-item" to="/account">Акаунт</Link>
                        </div>
                    </nav>
                )}
                {/*routes*/}
                <Routes>
                    <Route element={<ProtectedRoutes/>}>
                        <Route path="/" element={<Home/>}/>
                        <Route path="/account" element={<Account/>}/>
                    </Route>
                </Routes>
                {!this.state.isAuth && <SignIn onClose={() => this.auth()}/>}
            </div>
        );
    }
}
