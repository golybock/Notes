import React from 'react';
import Cat from "./resources/cat.webp"
import './App.css';
import UserApi from "./api/user/UserApi";
import {Link, Route, Routes} from "react-router-dom";
import Home from "./components/home/Home";
import Account from "./components/auth/Account";
import PageNotFound from "./components/codes/PageNotFound";
import Auth from "./components/auth/Auth";
import {Navbar} from "react-bootstrap";

export interface IProps {

}

export interface IState {
    isAuthed: boolean
}

export default class App extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);

        this.state = {
            isAuthed : false
        }
    
    }

    async componentDidMount() {

        let user = await UserApi.getUser();

        let isAuth = user != null;
        
        this.setState({isAuthed: isAuth})
    }

    auth() {
        this.setState({isAuthed: true});
    }

    render() {
        return (
            <div className="App">

                {/*nav bar*/}
                {this.state.isAuthed && (
                    <Navbar className="Nav-panel">

                        <Link className="Navbar-item" to="/">
                            <img src={Cat} alt={Cat} className="App-logo"/>
                        </Link>

                        <Link className="Navbar-item" to="/account">Акаунт</Link>

                    </Navbar>
                )}

                {/*main content*/}
                {this.state.isAuthed && (
                    <div>
                        <Routes>
                            <Route path="/" element={<Home/>}/>
                            <Route path="/account" element={<Account/>}/>
                            <Route path="*" element={<PageNotFound/>}/>
                        </Routes>
                    </div>
                )}

                {/*auth */}
                {!this.state.isAuthed && <Auth onClose={() => this.auth()}/>}

            </div>
        );
    }
}

