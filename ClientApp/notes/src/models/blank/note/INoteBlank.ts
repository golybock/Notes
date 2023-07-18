import INoteImageBlank from "./INoteImageBlank";
import {Guid} from "guid-typescript";

export default interface INoteBlank {
    header : string;
    text? : string;
    images? : Array<INoteImageBlank>;
    tags? : Array<Guid>;
}