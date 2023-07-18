import {Guid} from "guid-typescript";
import INoteTypeView from "./INoteTypeView";
import IUserView from "../user/IUserView";
import INoteImageView from "./INoteImageView";
import ITagView from "./ITagView";

export default interface INoteView{
    id : Guid;
    header : string;
    creationDate : Date;
    editedDate : Date;
    text : string;
    type : INoteTypeView;
    ownerUser : IUserView;
    sharedNotes : Array<IUserView>;
    images : Array<INoteImageView>;
    tags : Array<ITagView>;
}