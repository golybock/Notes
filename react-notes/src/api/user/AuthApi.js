import axios from 'axios';
import ApiBase from "../ApiBase";
import {LoginBlank} from "../../models/blank/user/LoginBlank";

export default class AuthApi extends ApiBase {

    static async login(email, password) {

        let url = this.baseAddress + "/Auth/Login";

        let blank = new LoginBlank(email, password);

        // let url = "https://localhost:7058/api/Auth/Login?email=aboba12%40aboba.com&password=Password1%21"

        this.deleteTokens()

        return await axios.post(url, blank)
            .then(async res => {
                if (res.status === 200) {

                    this.setTokens(res.data.token, res.data.refreshToken);

                    this.setAuthorization()

                    return true;
                }
                return false;
            })
            .catch(() => {
                return false;
            });
    }

    static async Registration(noteUserBlank) {

        let url = this.baseAddress + "/Auth/Registration";

        await axios.post(url, noteUserBlank)
            .then(async res => {
                if (res.status === 200) {

                    this.setTokens(res.data.token, res.data.refreshToken);

                    this.setAuthorization()

                    return true;
                }
                return false;
            })
            .catch(() => {
                return false;
            });
    }

}