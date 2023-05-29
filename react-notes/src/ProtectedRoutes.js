import AuthApi from "./api/user/AuthApi";
import {useLocation} from "react-router";
import {Navigate, Outlet} from "react-router-dom";

const ProtectedRoutes = () => {
    const location = useLocation();
    const isAuth = AuthApi.token() != null;
    return isAuth ? (
        <Outlet/>
    ) : (
        <Navigate to="/login" replace state={{from: location}}/>
    );
};

export default ProtectedRoutes;
