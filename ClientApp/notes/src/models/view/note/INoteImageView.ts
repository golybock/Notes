import {Guid} from "guid-typescript";

export default interface INoteImageView {
    id : Guid;
    x : number;
    y : number;
    width : number;
    height : number;
    url? : string;
}