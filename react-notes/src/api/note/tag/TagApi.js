import axios from 'axios';
import ApiBase from "../../ApiBase";
import {TagBlank} from "../../../models/blank/note/TagBlank";

export default class TagApi extends ApiBase{

    static async getTags() {

        let url = this.baseAddress + "/Tag/Tags";

        return await axios.get(url)
            .then(async res => {
                return await res.data;
            })
            .catch((e) => {
                return [];
            });
    }

    static async createTag(tag : TagBlank){
        let url = this.baseAddress + "/Tag/Tag"

        return await axios.post(url, tag)
            .then(async res => {
                return res.status === 200;
            })
            .catch((e) => {
                return false;
            });
    }
}