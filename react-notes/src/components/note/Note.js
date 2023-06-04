import React from "react";
import "./Note.css"
import NoteApi from "../../api/note/NoteApi";
import {NoteBlank} from "../../models/blank/note/NoteBlank";
import RichTextEditor from "react-rte";

export default class Home extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            id: this.props.id,
            note: new NoteBlank(-1),
            value: RichTextEditor.createValueFromString("", "html")
        };
    }

    async componentDidMount() {
        // check created note
        if (this.props.id === "null") {
            // create new note and get id
            await this.createNote();
            // load note data
            await this.loadNote();
        } else {
            // load exists note
            await this.loadNote();
        }
    }

    async loadNote() {
        // get note from api
        let note = await NoteApi.getNote(this.state.id);

        // save in state
        this.setState({note: note})

        // render text
        let text = RichTextEditor.createValueFromString(note.text, "html");

        // save text in html mode in state
        this.setState({value: text})
    }

    async update() {
        await NoteApi.updateNote(this.props.id, this.state.note)
    }

    async createNote() {

        let id = await NoteApi.createNote(this.state.note)

        this.setState({id: id})
    }

    onChange = async (value) => {

        if(this.state.note.header !== -1){
            // value in json
            this.setState({
                note: {
                    ...this.state.note,
                    text: value.toString("html")
                }
            })

            // rendered value
            this.setState({value: value})

            await this.update()
        }
    }

    render() {
        return (<div>
                <div className="buttons">
                    <button className="btn btn-primary-note">
                        Share
                    </button>
                    <button className="btn btn-primary-delete">
                        Delete
                    </button>
                    <button className="btn btn-primary-note" onClick={this.props.onClose}>
                        Back
                    </button>
                </div>
                <RichTextEditor value={this.state.value} onChange={this.onChange}/>
            </div>
        );
    }
}
