import { Guid } from "guid-typescript";

export default interface INoteImageBlank{
    id : Guid;
    x : number;
    y : number;
    width : number;
    height : number;
}