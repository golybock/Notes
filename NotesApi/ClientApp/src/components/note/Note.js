import React from "react";
import "./Note.css"
import NoteApi from "../../api/note/NoteApi";
import {NoteBlank} from "../../models/blank/note/NoteBlank";
import RichTextEditor from "react-rte";
import DeleteNoteDialog from "./DeleteNoteDialog";
import ShareNoteDialog from "./ShareNoteDialog";

export default class Note extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            id: this.props.id,
            note: new NoteBlank("", ""),
            value: RichTextEditor.createValueFromString("", "html"),
            show_dialog : false
        };
    }

    async componentDidMount() {
        await this.loadNote();
    }
    
    showDialog(){
        this.setState({show_dialog : true})
    }
    
    closeDialog(){
        this.setState({show_dialog : false})
    }
    
    async loadNote() {
        // get note from api
        let note = await NoteApi.getNote(this.state.id);

        if (note !== null) {
            // save in state
            this.setState({note: note})

            // render text
            let text = RichTextEditor.createValueFromString(note.text, "html");

            // save text in html mode in state
            this.setState({value: text})
        }
    }

    // update note
    async update() {
        await NoteApi.updateNote(this.props.id, this.state.note)
    }

    // onchange text
    onChange = async (value) => {

        if (this.state.note.header !== -1) {
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

                {/*command bar*/}
                <div className="buttons">
                    <button className="btn btn-primary-note">
                        Share
                    </button>
                    <button className="btn btn-primary-note" onClick={() => this.showDialog()}>
                        Delete
                    </button>
                    <button className="btn btn-primary-note" onClick={this.props.onClose}>
                        Back
                    </button>
                </div>

                {/*text editor*/}
                <RichTextEditor value={this.state.value} onChange={this.onChange}/>

                <DeleteNoteDialog show={this.state.show_dialog}
                                  id={this.state.note.id}
                                  name={this.state.note.header}
                                  onCloseDialog={() => this.closeDialog()}
                                  onClose={() => this.props.onClose()}/>
                
                <ShareNoteDialog/>
                
            </div>
        );
    }
}
