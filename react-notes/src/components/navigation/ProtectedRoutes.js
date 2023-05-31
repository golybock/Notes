import AuthApi from "../../api/user/AuthApi";
import {useLocation} from "react-router";
import {Navigate, Outlet} from "react-router-dom";

const useAuth = () => {
    return AuthApi.token() != null;
};

const ProtectedRoutes = () => {
    const location = useLocation();
    const isAuth = useAuth();
    return isAuth ? (
        <Outlet/>
    ) : (
        <Navigate to="/" replace state={{from: location}}/>
    );
};

export default ProtectedRoutes;
