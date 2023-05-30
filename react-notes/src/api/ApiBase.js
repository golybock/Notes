import Cookies from "universal-cookie";
import {TokensBlank} from "../models/blank/user/TokensBlank";
import axios from "axios";

export default class ApiBase {


    static baseAddress = "https://localhost:7058/api"

    static cookies = new Cookies();

    static token = () => {
        try {
            return this.cookies.get('token').toString()
        } catch {
            return null;
        }
    }

    static setAuthorization() {
        if(this.token == null){
        }
        else{
            axios.defaults.headers.common['Authorization'] = 'Bearer ' + this.token();
        }
    }

    static deleteTokens() {
        this.cookies.remove("refreshToken", {path: '/'})
        this.cookies.remove("token", {path: '/'})
    }

    static setTokens(token, refreshToken) {
        this.cookies.set("token", token, {path: '/'})
        this.cookies.set("refreshToken", refreshToken, {path: '/'})
    }

    static getTokens() {

        try {
            let token = this.cookies.get('token')
            let refreshToken = this.cookies.get('refreshToken')
            return new TokensBlank(token, refreshToken);
        } catch {
            return null;
        }
    }

    static async refreshTokens() {

        let url = this.baseAddress + "/Auth/RefreshTokens";

        let tokens = this.getTokens()

        return await axios.post(url, tokens)
            .then(res => {
                if (res.status === 200) {

                    this.deleteTokens()

                    this.setTokens(res.data.token, res.data.refreshToken)

                    this.setAuthorization()

                    return true;
                }

                return false;
            })
            .catch((e) => {
                console.log(e)
                return false;
            });
    }
}