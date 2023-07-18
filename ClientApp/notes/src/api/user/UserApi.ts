import axios from 'axios';
import ApiBase from "../ApiBase";
import IUserView from "../../models/view/user/IUserView";

export default class UserApi extends ApiBase {

    static async getUser() : Promise<IUserView> {

        axios.defaults.withCredentials = true
        
        let url = this.baseAddress + "/User/User";

        return await axios.get(url, {withCredentials: true})
            .then(async res => {
                return res.data;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }

}