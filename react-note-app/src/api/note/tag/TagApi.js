import axios from 'axios';
import ApiBase from "../../ApiBase";

export default class TagApi extends ApiBase{

    static async getTags() {

        let url = this.baseAddress + "/Tag/Tags";

        return await axios.get(url)
            .then(async res => {
                return await res.data;
            })
            .catch((e) => {
                alert(e)
                return false;
            });
    }
}