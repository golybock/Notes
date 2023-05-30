import React from "react";
import "./Note.css"
import NoteApi from "../../api/note/NoteApi";
import Button from "react-bootstrap/Button";
import {NoteBlank} from "../../models/blank/note/NoteBlank";
import RichTextEditor from "react-rte";

export default class Home extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            note: new NoteBlank(),
            value: RichTextEditor.createValueFromString("", "html")
        };
    }

    async componentDidMount() {
        if (this.props.guid != null) {
            await this.editMode();
        } else {
            this.createMode();
        }
    }

    async editMode() {
        // get note from api
        let note = await NoteApi.getNote(this.props.guid);

        this.setState({note: note})

        // render text
        let text = RichTextEditor.createValueFromString(note.text, "html");

        this.setState({value: text})
    }

    createMode() {
        let note = new NoteBlank("", "")

        this.setState({note: note})

        // render text
        let text = RichTextEditor.createValueFromString(note.text, "html");

        this.setState({value: text})
    }

    async update(){
        await NoteApi.updateNote(this.props.guid, this.state.note)
    }

    async create(){
        await NoteApi.createNote(this.state.note)
    }

    onChange = (value) => {

        // value in json
        this.setState({
            note: {
                ...this.state.note,
                text: value.toString("html")
            }
        })

        // rendered value
        this.setState({value: value})
    }

    render() {
        return (<div>
                <p>{this.props.guid == null ? "create" : "edit"}</p>
                {this.props.guid}
                <Button onClick={this.props.onClose}>Закрыть</Button>
                <RichTextEditor value={this.state.value} onChange={this.onChange}/>
                <button
                    onClick={async () => {
                        if (this.props.guid == null) {
                            await this.create()
                        }
                        else{
                            await this.update()
                        }
                    }}
                >
                    Submit
                </button>
                <p>{this.state.note.text}</p>
            </div>
        );
    }
}
