import React from "react";
import "./NoteCardsList.css"
import NoteCard from "./NoteCard";

export default class NoteCardsList extends React.Component{

    render() {
        return(
            <div>

                <h1 style={{textAlign: "left", marginLeft: '20px', marginTop: '10px'}}>{this.props.header}</h1>

                <div className="cards">

                    {this.props.notes
                        .map(note =>
                            <NoteCard note={note} open={this.props.open}/>)
                    }

                </div>

            </div>
        )
    }

}