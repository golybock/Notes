import INoteView from "./view/note/INoteView";
import INoteBlank from "./blank/note/INoteBlank";
import {Guid} from "guid-typescript";
import INoteImageBlank from "./blank/note/INoteImageBlank";
import INoteImageView from "./view/note/INoteImageView";

export default class NoteBuilder {
    
    static convertViewToBlank(noteView : INoteView) : INoteBlank{

        let blank : INoteBlank = {
            header : noteView.header,
            text : noteView.text,
            tags : [],
        }

        // set tags
        let arr : Array<Guid> = []

        noteView.tags.forEach(element => {
            arr.push(element.id);
        });

        blank.tags = arr
        
        let images : Array<INoteImageBlank> = []

        noteView.images.forEach(element => {
            images.push(this.convertImageViewToBlank(element));
        });

        blank.images = images
        
        return blank;
    }
    
    static convertImageViewToBlank(noteImageView : INoteImageView) : INoteImageBlank{
        return {
            id: noteImageView.id,
            x: noteImageView.x,
            y: noteImageView.y,
            height: noteImageView.height,
            width: noteImageView.width
        };
    }
    
}