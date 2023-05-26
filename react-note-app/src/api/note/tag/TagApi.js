import axios from 'axios';
import ApiBase from "../../ApiBase";

export default class TagApi extends ApiBase{

    static async getTags() {

        // let url = this.baseAddress + "/Tag/Tags";

        let url = "http://localhost:5133/api/Tag/Tags"

        this.setAuthorization()

        return await axios.get(url)
            .then(async res => {
                return await res.data;
            })
    }
}