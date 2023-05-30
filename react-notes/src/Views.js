import {Route, Routes} from "react-router-dom";
import React from "react";
import Account from "./components/Account";
import Home from "./components/home/Home";
import Note from "./components/note/Note";
import ProtectedRoutes from "./ProtectedRoutes";

export default class Views extends React.Component {

    render() {
        return (
            <Routes>
                <Route element={<ProtectedRoutes/>}>
                    <Route path="/" element={<Home/>}/>
                    <Route path="/account" element={<Account/>}/>
                    <Route path="/create" element={<Note/>}/>
                </Route>
            </Routes>
        );
    }
};