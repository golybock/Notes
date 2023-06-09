import axios from 'axios';
import ApiBase from "../ApiBase";
import {LoginBlank} from "../../models/blank/user/LoginBlank";

export default class AuthApi extends ApiBase {

    static async signIn(email, password) {

        let url = this.baseAddress + "/Auth/SignIn";

        let blank = new LoginBlank(email, password);

        return await axios.post(url, blank)
            .then(async res => {
                return res.status === 200;
            })
            .catch(() => {
                return false;
            });
    }

    static async signUp(noteUserBlank) {

        let url = this.baseAddress + "/Auth/SignUp";

        return await axios.post(url, noteUserBlank)
            .then(async res => {
                return res.status === 200;
            })
            .catch(() => {
                return false;
            });
    }

    static async signOut() {

        let url = this.baseAddress + "/Auth/SignOut";

        return await axios.post(url)
            .then(async res => {
                return res.status === 200;
            })
            .catch(() => {
                return false;
            });
    }

}