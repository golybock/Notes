import './App.css';
import React from "react";
import {Link, Route, Routes} from "react-router-dom";
import Home from "./components/home/Home";
import Account from "./components/Auth/Account";
import Cat from "./resources/cat.webp"
import UserApi from "./api/user/UserApi";
import Auth from "./components/Auth/Auth";
import PageNotFound from "./components/codes/PageNotFound";

export default class App extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            isAuthed: false
        }
    }

    async componentDidMount() {

        let user = await UserApi.getUser();

        let isAuth = user != null;

        this.setState({isAuth: isAuth})
        this.setState({login: !isAuth})
    }

    auth() {
        this.setState({isAuth: true});
    }
    
    render() {
        return (
            <div className="App">

                {/*nav bar*/}
                {this.state.isAuth && (
                    <nav className="Nav-panel">
                        <Link className="Navbar-item" to="/">
                            <img src={Cat} alt={Cat} className="App-logo"/>
                        </Link>
                        {/*<Link className="Navbar-item" to="/">Главная</Link>*/}
                        <Link className="Navbar-item" to="/account">Акаунт</Link>
                    </nav>
                )}

                {/*main content*/}
                {this.state.isAuth && (
                    <div>
                        {/*routes*/}
                        <Routes>
                            <Route path="/" element={<Home/>}/>
                            <Route path="/account" element={<Account/>}/>
                            <Route path="*" element={<PageNotFound/>} />
                        </Routes>
                    </div>
                )}

                {/*auth */}
                {!this.state.isAuth && <Auth onClose={() => this.auth()}/>}

            </div>
        );
    }
}