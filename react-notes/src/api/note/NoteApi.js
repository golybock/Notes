import axios from 'axios';
import ApiBase from "../ApiBase";
import type {NoteBlank} from "../../models/blank/note/NoteBlank";

export default class NoteApi extends ApiBase {

    static async getNotes() {

        await this.refreshTokens();

        let url = this.baseAddress + "/Note/Notes";

        return await axios.get(url)
            .then(async res => {
                return await res.data;
            })
            .catch((e) => {
                console.log(e)
                return [];
            });
    }

    static async getSharedNotes() {

        await this.refreshTokens();

        let url = this.baseAddress + "/Note/SharedNotes";

        return await axios.get(url)
            .then(async res => {
                return await res.data;
            })
            .catch((e) => {
                console.log(e)
                return [];
            });
    }

    static async getNote(guid) {

        await this.refreshTokens();

        let url = this.baseAddress + "/Note/Note?guid=" + guid;

        return await axios.get(url)
            .then(async res => {
                return await res.data;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }

    static async createNote(noteBlank : NoteBlank) {

        await this.refreshTokens();

        let url = this.baseAddress + "/Note/Note";

        return await axios.post(url, noteBlank)
            .then(async res => {
                return await res.data;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }

    static async shareNote(shareBlank : ShareNoteBlank) {

        await this.refreshTokens();

        let url = this.baseAddress + "/Note/Share";

        return await axios.post(url, shareBlank)
            .then(async res => {
                return await res.data;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }

    static async updateNote(guid, noteBlank) {

        await this.refreshTokens();

        let url = this.baseAddress + "/Note/Note?guid=" + guid;

        return await axios.put(url, noteBlank)
            .then(async res => {
                return await res.data;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }
}