import axios from 'axios';
import Cookies from "universal-cookie";
import {TokensBlank} from "../../models/blank/user/TokensBlank";

class AuthApi {

    static async login(email, password) {

        let url = "https://localhost:7058/api/Auth/Login?email=" + email + "&password=" + password;

        let cookies = new Cookies();

        cookies.remove("refreshToken", {path: '/'})
        cookies.remove("token", {path: '/'})

        return await axios.post(url)
            .then(async res => {
                if (res.status === 200) {

                    cookies.set("token", res.data.token, {path: '/'})
                    cookies.set("refreshToken", res.data.refreshToken, {path: '/'})

                    // localStorage.setItem("token", res.data.token)
                    // localStorage.setItem("refreshToken", res.data.refreshToken)

                    return true;
                }
                return false;
            }).catch(() => {
                return false;
            });
    }

    static async Registration(noteUserBlank) {

        let url = "https://localhost:7058/api/Auth/Registration";

        let res = await axios.post(url, noteUserBlank)
            .then(async res => {
                return res;
            })

        return res.status === 200;
    }

    static async RefreshTokens() {

        let url = "https://localhost:7058/api/Auth/RefreshTokens";

        const cookies = new Cookies();

        let token = cookies.get('token')
        let refreshToken = cookies.get('refreshToken')

        let refreshTokens = new TokensBlank(token, refreshToken)

        let res = await axios.post(url, refreshTokens)
            .then(async res => {
                if (res.status === 200) {

                    cookies.remove("refreshToken", {path: '/'})
                    cookies.remove("token", {path: '/'})

                    cookies.set("token", res.data.token, {path: '/'})
                    cookies.set("refreshToken", res.data.refreshToken, {path: '/'})

                    // localStorage.setItem("token", res.data.token)
                    // localStorage.setItem("refreshToken", res.data.refreshToken)

                    return true;
                }
                return false;
            }).catch(() => {
                return false;
            });

        return res.status === 200;
    }
}

export default AuthApi