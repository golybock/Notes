import React from "react";
import "./NoteCardsList.css"
import NoteCard from "./NoteCard";
import INoteView from "../../models/view/note/INoteView";

interface IProps{
    header : string;
    notes : Array<INoteView>
    open : Function
}

interface IState{
}

export default class NoteCardsList extends React.Component<IProps, IState>{
    
    render() {
        return(
            <div>

                <h1 style={{textAlign: "left", marginLeft: '20px', marginTop: '10px'}}>{this.props.header}</h1>

                <div className="cards">

                    {this.props.notes
                        .map(note =>
                            <NoteCard key={note.id.toString()} note={note} open={this.props.open}/>)
                    }

                </div>

            </div>
        )
    }

}