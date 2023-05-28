import './App.css';
import {createContext, useState} from "react";
import LogInButtons from "./components/LogInButtons";
import Navigation from "./components/navigation/Navigation";
import Views from "./Views";

export const UserContext = createContext(this);

function App() {
    const [user, setUser] = useState({loggedIn: false});
    return (
        <UserContext.Provider value={{user, setUser}}>
            <Navigation/>
            <LogInButtons/>
            <Views/>
        </UserContext.Provider>
    );
}

export default App;
