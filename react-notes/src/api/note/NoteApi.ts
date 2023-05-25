import axios from 'axios';

class NoteApi {

    static async getNotes() {
        return await axios.get("https://localhost:7058/api/Note/Notes")
            .then(async res => {
                return await res.data;
            })
    }
}

export default NoteApi