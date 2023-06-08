import './App.css';
import React from "react";
import SignIn from "./components/Auth/SignIn";
import {Link, Route, Routes} from "react-router-dom";
import Home from "./components/home/Home";
import Account from "./components/Account";
import Cat from "./cat.webp";
import UserApi from "./api/user/UserApi";
import Registration from "./components/Auth/Registration";

export default class App extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            isAuthed: false,
            registration: false,
            login : false
        }
    }

    async componentDidMount() {

        let user = await UserApi.getUser();

        let isAuth = user != null;

        this.setState({isAuth: isAuth})
        this.setState({login: isAuth})
    }

    auth() {
        this.setState({isAuth: true});
    }
    
    setRegistration(){
        this.setState({login: false})
        this.setState({registration: true})
    }
    
    setLogin(){
        this.setState({login: true})
        this.setState({registration: false})
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
                            {/*<Link className="Navbar-item" to="/">Главная</Link>*/}
                            <Link className="Navbar-item" to="/account">Акаунт</Link>
                        </div>
                    </nav>
                )}

                {this.state.isAuth && (
                    <div>
                        {/*routes*/}
                        <Routes>
                            <Route path="/" element={<Home/>}/>
                            <Route path="/account" element={<Account/>}/>
                        </Routes>
                    </div>
                )}

                {/*login*/}
                {!this.state.isAuth && <SignIn onClose={() => this.auth()} onRegistration={() => this.setRegistration()}/>}

                {/*registration*/}
                {!this.state.isAuth && <Registration onClose={() => this.auth()} onLogin={() => this.setLogin()}/>}
                
            </div>
        );
    }
}
