import axios from 'axios';
import ApiBase from "../ApiBase";

export default class NoteApi extends ApiBase {

    static async getNotes() {

        await this.refreshTokens();

        let url = this.baseAddress + "/Note/Notes";

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