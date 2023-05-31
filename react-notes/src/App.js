import './App.css';
import React from "react";
import AuthApi from "./api/user/AuthApi";
import SignIn from "./components/Auth/SignIn";
import {Link, Route, Routes} from "react-router-dom";
import ProtectedRoutes from "./components/navigation/ProtectedRoutes";
import Home from "./components/home/Home";
import Account from "./components/Account";
import Note from "./components/note/Note";
import Cat from "./cat.webp";

export default class App extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            isAuth: false
        }
    }

    componentDidMount() {
        this.setState({isAuth: AuthApi.token() != null})
    }

    auth(){
        this.setState({isAuth: true});
    }

    render() {
        return (
            <div className="App">
                {this.state.isAuth && (<nav>
                    <div className="Nav-panel">
                        <Link className="Navbar-item" to="/">
                            <img src={Cat} alt={Cat} className="App-logo"/>
                        </Link>
                        <Link className="Navbar-item" to="/">Главная</Link>
                        <Link className="Navbar-item" to="/account">Акаунт</Link>
                        <Link className="Navbar-item" to="/note">Создать</Link>
                    </div>
                </nav>)}
                <Routes>
                    <Route element={<ProtectedRoutes/>}>
                        <Route path="/" element={<Home/>}/>
                        <Route path="/account" element={<Account/>}/>
                        <Route path="/note" element={<Note/>}/>
                    </Route>
                </Routes>
                {!this.state.isAuth && <SignIn onClose={() => this.auth()}/>}
            </div>
        );
    }
}
