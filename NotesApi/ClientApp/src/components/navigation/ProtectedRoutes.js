// import {useLocation} from "react-router";
// import {Navigate, Outlet} from "react-router-dom";
// import UserApi from "../../api/user/UserApi";
//
// const useAuth = async () => {
//     let user = await UserApi.getUser();
//
//     return  user != null;
//
// };
//
// const ProtectedRoutes = () => {
//     const location = useLocation();
//     const isAuth = useAuth();
//     return isAuth ? (
//         <Outlet/>
//     ) : (
//         <Navigate to="/" replace state={{from: location}}/>
//     );
// };
//
// export default ProtectedRoutes;
