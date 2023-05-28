import { Route, Routes } from "react-router-dom";
import Account from "./components/Account";
import Home from "./components/Home";
import SignIn from "./components/Auth/SignIn";
import Registration from "./components/Auth/Registration";
import ProtectedRoutes from "./ProtectedRoutes";

const Views = () => {
    return (
        <Routes>
            <Route path="/login" element={<SignIn />} />
            <Route path="/registration" element={<Registration />} />
            <Route element={<ProtectedRoutes />}>
                <Route path="/" element={<Home />} />
                <Route path="/home" element={<Home />} />
                <Route path="/account" element={<Account />} />
            </Route>
        </Routes>
    );
};

export default Views;
