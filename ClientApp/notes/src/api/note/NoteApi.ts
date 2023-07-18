import axios from 'axios';
import ApiBase from "../ApiBase";
import FormData from "form-data";
import INoteBlank from "../../models/blank/note/INoteBlank";
import INoteView from "../../models/view/note/INoteView";
import IShareBlank from "../../models/blank/note/share/IShareBlank";
import {Guid} from "guid-typescript";

export default class NoteApi extends ApiBase {

    static async getNotes(): Promise<Array<INoteView>> {

        let url = this.baseAddress + "/Note/Notes";

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }

    static async getSharedNotes(): Promise<Array<INoteView>> {

        let url = this.baseAddress + "/Note/SharedNotes";

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch((e) => {
                console.log(e)
                return [];
            });
    }

    static async getNote(id : Guid) : Promise<INoteView> {

        let url = this.baseAddress + "/Note/Note?id=" + id;

        return await axios.get(url)
            .then(async res => {
                return res.data;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }

    static async createNote(noteBlank: INoteBlank) : Promise<boolean> {

        let url = this.baseAddress + "/Note/Note";

        return await axios.post(url, noteBlank)
            .then(async res => {
                return res.data;
            })
            .catch((e) => {
                console.log(e)
                return null;
            });
    }

    static async shareNote(shareBlank: IShareBlank) : Promise<boolean> {

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

    static async updateNote(id : Guid, noteBlank : INoteBlank) : Promise<boolean> {

        let url = this.baseAddress + "/Note/Note?id=" + id;

        return await axios.put(url, noteBlank)
            .then(async res => {
                return res.status === 200;
            })
            .catch((e) => {
                console.log(e)
                return false;
            });
    }

    static async uploadFile(file : File, noteId : Guid) : Promise<Guid> {

        let url = this.baseAddress + '/Note/Image?noteId=' + noteId;

        let formData = new FormData()

        formData.append("image", file, file.name)

        return await axios.post(url, formData)
            .then(async res => {
                return res.data;
            })
            .catch((e) => {
                return null;
            })
    }

    static async deleteNote(id : Guid) : Promise<boolean> {

        let url = this.baseAddress + "/Note/Note?id=" + id;

        return await axios.delete(url)
            .then(async res => {
                return res.status === 200;
            })
            .catch((e) => {
                console.log(e)
                return false;
            });
    }
}