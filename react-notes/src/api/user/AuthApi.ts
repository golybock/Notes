import axios from 'axios';

class NoteApi {

    static async login(email: string, password: string) {

        let url = "https://localhost:7058/api/Auth/Login?email=" + email + "&password=" + password;

        let res = await axios.post(url)
            .then(async res => {
                return res;
            })

        return res.status === 200;
    }

    static async Registration(email : string, password : string) {

        let url = "https://localhost:7058/api/Auth/Login?email=" + email + "&password=" + password;

        let res = await axios.post(url)
            .then(async res => {
                return res;
            })

        return res.status === 200;
    }
}

export default NoteApi