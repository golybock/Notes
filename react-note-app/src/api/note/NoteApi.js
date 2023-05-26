import axios from 'axios';
import Cookies from "universal-cookie";
import ApiBase from "../ApiBase";

export default class NoteApi extends ApiBase{

    static async getNotes() {

        await this.refreshTokens();

        return await axios.get("https://localhost:7058/api/Note/Notes")
            .then(async res => {
                return await res.data;
            })
    }
}