import axios from 'axios';
import Cookies from "universal-cookie";
import AuthApi from "../user/AuthApi";

class NoteApi {

    static async getNotes() {

        await AuthApi.RefreshTokens();

        const cookies = new Cookies();

        let token = cookies.get('token')

        axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;

        return await axios.get("https://localhost:7058/api/Note/Notes")
            .then(async res => {
                return await res.data;
            })
    }
}

export default NoteApi