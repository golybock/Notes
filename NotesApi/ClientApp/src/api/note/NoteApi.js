import axios from 'axios';
import ApiBase from "../ApiBase";
import {NoteBlank} from "../../models/blank/note/NoteBlank";
import type ShareNoteBlank from "../../models/blank/note/ShareNoteBlank";

export default class NoteApi extends ApiBase {

    static async getNotes() {

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

    static async createNote(noteBlank: NoteBlank) {

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

    static async shareNote(shareBlank: ShareNoteBlank) {

        let url = this.baseAddress + "/Note/Share";

        return await axios.post(url, shareBlank)
            .then(async res => {
                return res.status === 200;
            })
            .catch((e) => {
                console.log(e)
                return false;
            });
    }

    static async updateNote(guid, noteBlank) {

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
    
    static async uploadFile(){
        
    }

    static async deleteNote(guid) {

        let url = this.baseAddress + "/Note/Note?guid=" + guid;

        return await axios.delete(url)
            .then(async res => {
                return res.status === 200;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }
}