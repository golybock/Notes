import './App.css';
import React from "react";
import SignIn from "./components/Auth/SignIn";
import {Link, Route, Routes} from "react-router-dom";
import ProtectedRoutes from "./components/navigation/ProtectedRoutes";
import Home from "./components/home/Home";
import Account from "./components/Account";
import Cat from "./cat.webp";
import UserApi from "./api/user/UserApi";
// import {gapi} from "gapi-script";

export default class App extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            isAuth: false,
        }
    }

    async componentDidMount() {

        let user = await UserApi.getUser();

        let isAuth = user != null;

        this.setState({isAuth: isAuth})
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
                    <Route path="/" element={<Home/>}/>
                    <Route path="/account" element={<Account/>}/>
                    {/*<Route element={<ProtectedRoutes/>}>*/}
                    {/*</Route>*/}
                </Routes>

                {/*login*/}
                {!this.state.isAuth && <SignIn onClose={() => this.auth()}/>}

            </div>
        );
    }
}
