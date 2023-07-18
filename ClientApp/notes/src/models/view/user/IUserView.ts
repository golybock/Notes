import {Guid} from "guid-typescript";

export default interface IUserView{
    id : Guid;
    name : string;
    email : string;
}