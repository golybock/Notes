import React, {useEffect} from "react";
import "./Note.css"
import NoteApi from "../../api/note/NoteApi";
import {NoteBlank} from "../../models/blank/note/NoteBlank";
import RichTextEditor from "react-rte";
import DeleteNoteDialog from "./dialogs/DeleteNoteDialog";
import ShareNoteDialog from "./dialogs/ShareNoteDialog";
import TagDialog from "./TagsDialog";
import ImagesLayer from "./ImagesLayer";
import {Await} from "react-router";

export default class Note extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            id: this.props.id,
            note: new NoteBlank("", "", []),
            // noteView : null,
            value: RichTextEditor.createValueFromString("", "html"),
            show_dialog_delete: false,
            show_dialog_share: false,
            show_tags: false
        };

        this.intervalId = setInterval(async () => {
            await this.update();
            console.log("timer")
        }, 3000);
    }

    async componentDidMount() {

        await this.loadNote();
    }

    componentWillUnmount(){
        clearInterval(this.intervalId);
    }

    showDialogDelete() {
        this.setState({show_dialog_delete: true})
    }

    closeDialogDelete() {
        this.setState({show_dialog_delete: false})
    }

    showDialogShare() {
        this.setState({show_dialog_share: true})
    }

    closeDialogShare() {
        this.setState({show_dialog_share: false})
    }

    showTags() {
        this.setState({show_tags: true})
    }

    async closeTags() {
        this.setState({show_tags: false})

        await this.loadNote()
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

        let note = this.state.note

        let arr = []

        this.state.note.tags.forEach(element => {
            arr.push(element.id);
        });

        note.tags = arr

        await NoteApi.updateNote(this.props.id, note)
    }

    async uploadImage() {

        await this.update();

        await this.loadNote();
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
        }
    }

    onChangeName = async (value) => {

        this.setState({
            note: {
                ...this.state.note,
                header: value
            }
        })
    }

    render() {
        return (<div>

                {/*command bar*/}
                <div className="buttons">

                    <label style={{margin: "5px", fontSize: "24px"}}> Заголовок:</label>

                    <input className="form-control"
                           style={{width: "10rem", margin: "5px"}}
                           value={this.state.note.header}
                           onChange={
                               async (e) =>
                                   await this.onChangeName(e.target.value)
                           }/>

                    <button className="form-control btn btn-secondary"
                            style={{width: "10rem"}}
                            onClick={() => this.showDialogShare()}>
                        Share
                    </button>

                    <button className="form-control btn btn-secondary"
                            style={{width: "10rem"}}
                            onClick={() => this.showDialogDelete()}>
                        Delete
                    </button>

                    <button className="form-control btn btn-secondary"
                            style={{width: "10rem"}}
                            onClick={() => this.showTags()}>
                        Tags
                    </button>

                    <button className="form-control btn btn-secondary"
                            style={{width: "10rem"}}
                            onClick={this.props.onClose}>
                        Close
                    </button>

                </div>

                {/*text editor*/}
                <RichTextEditor editorClassName="editor"
                                toolbarClassName="editor"
                                placeholder="Начните ввод..."
                                value={this.state.value}
                                onChange={this.onChange}/>

                {/*dialogs*/}

                {this.state.show_dialog_delete &&

                    <DeleteNoteDialog show={this.state.show_dialog_delete}
                                      note={this.state.note}
                                      onCloseDialog={() => this.closeDialogDelete()}
                                      onClose={() => this.props.onClose()}/>

                }

                {this.state.show_dialog_share &&

                    <ShareNoteDialog show={this.state.show_dialog_share}
                                     note={this.state.note}
                                     onCloseDialog={() => this.closeDialogShare()}/>
                }

                {this.state.show_tags &&

                    <TagDialog show={this.state.show_tags}
                               note={this.state.note}
                               update={async () => await this.update()}
                               onCloseDialog={async () => await this.closeTags()}/>

                }

                {/*todo need refactor*/}
                {/*{this.state.note.images &&*/}
                {/*    <ImagesLayer images={this.state.note.images}*/}
                {/*                 note={this.state.note}*/}
                {/*                 uploadImage={async () => await this.uploadImage()}/>*/}
                {/*}*/}

            </div>
        );
    }
}
