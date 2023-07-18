import axios from 'axios';
import ApiBase from "../ApiBase";
import ILoginBlank from "../../models/blank/user/ILoginBlank";
import IUserBlank from "../../models/blank/user/IUserBlank";

export default class AuthApi extends ApiBase {

    static async signIn(email: string, password: string): Promise<boolean> {

        let url = this.baseAddress + "/Auth/SignIn";

        let blank: ILoginBlank = {email, password};

        return await axios.post(url, blank)
            .then(async res => {
                return res.status === 200;
            })
            .catch(() => {
                return false;
            });
    }

    static async signUp(UserBlank: IUserBlank): Promise<boolean> {

        let url = this.baseAddress + "/Auth/SignUp";

        return await axios.post(url, UserBlank)
            .then(async res => {
                return res.status === 200;
            })
            .catch(() => {
                return false;
            });
    }

    static async signOut(): Promise<boolean> {

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