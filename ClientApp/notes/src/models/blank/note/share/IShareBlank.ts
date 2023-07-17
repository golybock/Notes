import { Guid } from "guid-typescript";

export default interface IShareBlank{
    noteId : Guid;
    email : string;
    permissionLevel : number;
}