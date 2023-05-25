import './App.css';
import React from "react";
import {Notes} from "./components/notes/Notes"
import {Login} from "./components/auth/Login";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link,
} from 'react-router-dom';

export default class App extends React.Component {

  render() {
    return (
        <Router>
          <div>

            <nav>
              <div className="Nav-panel">
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