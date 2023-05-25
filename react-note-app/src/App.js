import './App.css';
import Cat from "./cat.webp"
import React from "react";
import Notes from "./components/notes/Notes"
import Login from "./components/Auth/Login"
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Link,
} from 'react-router-dom';

class App extends React.Component {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <Router>
                <div>

                    <nav>
                        <div className="Nav-panel">
                            <Link className="Navbar-item" to="/">
                                <img src={Cat} alt={Cat} className="App-logo"/>
                            </Link>
                            <Link className="Navbar-item" to="/">Главная</Link>
                            <Link className="Navbar-item" to="/login">Авторизация</Link>
                        </div>
                    </nav>

                    <Routes>
                        <Route path="/" element={<Notes/>}/>
                        <Route path="/login" element={<Login/>}/>
                    </Routes>

                </div>
            </Router>
        );
    }

}

export default App;
