import axios from 'axios';
import ApiBase from "../../ApiBase";
import ITagBlank from "../../../models/blank/note/tag/ITagBlank";
import ITagView from "../../../models/view/note/ITagView";

export default class TagApi extends ApiBase {

    static async getTags(): Promise<Array<ITagView>> {

        let url = this.baseAddress + "/Tag/Tags";

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch((e) => {
                return [];
            });
    }

    static async createTag(tag: ITagBlank): Promise<boolean> {
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