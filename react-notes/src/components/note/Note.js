import React, {useState} from "react";
import "./Note.css"
import NoteApi from "../../api/note/NoteApi";
import TagApi from "../../api/note/tag/TagApi";
import Button from "react-bootstrap/Button";
import Rte from "./Rte"
import {NoteBlank} from "../../models/blank/note/NoteBlank";
export default class Home extends React.Component {

    async componentDidMount() {
        this.setState({note: await NoteApi.getNote(this.props.guid)})
        console.log(this.state.note)
    }

    constructor(props) {
        super(props);
        this.state = {
            note : new NoteBlank()
        };
    }

    onChange = (value) => {
        console.log(value);
        this.setState({
            note: {                   // object that we want to update
                ...this.state.note,    // keep all other key-value pairs
                text: value       // update the value of specific key
            }
        })
    }

    render() {
        return (<div>

                {this.props.guid}
                <Button onClick={this.props.onClose}>Закрыть</Button>
                <Rte value={this.state.note.text} markup="" onChange={this.onChange}/>
                <button
                    onClick={() => {
                        console.log(this.state.note.text);
                        alert(this.state.note.text);
                    }}
                >
                    Submit
                </button>
                <p dangerouslySetInnerHTML={{ __html: this.state.note.text }}></p>
                <hr />
                <p>{this.state.note.text}</p>
            </div>
        );
    }
}
