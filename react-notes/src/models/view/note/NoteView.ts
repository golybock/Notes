import {TagView} from "./TagView";

export class NoteView {
    header : string = "";
    creationDate : string = "";
    editedDate : string = "";
    text : string = "";
    userId : number = 0;
    guid : string = "";
    tags : TagView[] = [new TagView()]
}