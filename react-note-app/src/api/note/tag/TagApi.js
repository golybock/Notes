import axios from 'axios';

class TagApi {

    static async getTags() {
        return await axios.get("https://localhost:7058/api/Tag/Tags")
            .then(async res => {
                return await res.data;
            })
    }
}

export default TagApi