import React, {useEffect} from "react";
import "./Note.css"
import NoteApi from "../../api/note/NoteApi";
import INoteBlank from "../../models/blank/note/INoteBlank";
import INoteView from "../../models/view/note/INoteView";
import RichTextEditor, {EditorValue} from "react-rte";
import DeleteNoteDialog from "./dialogs/DeleteNoteDialog";
import ShareNoteDialog from "./dialogs/ShareNoteDialog";
import TagDialog from "./dialogs/TagsDialog";
import {Guid} from "guid-typescript";
import NoteBuilder from "../../models/noteBuilder";

interface IProps {
    id: Guid;
    onClose: Function;
}

interface IState {
    noteView?: INoteView;
    noteBlank?: INoteBlank;
    rteValue: EditorValue;
    show_dialog_delete: boolean;
    show_dialog_share: boolean;
    show_tags: boolean;
    interval?: NodeJS.Timer;
}

export default class Note extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);

        this.state = {
            noteView: undefined,
            noteBlank: undefined,
            rteValue: RichTextEditor.createValueFromString("", "html"),
            show_dialog_delete: false,
            show_dialog_share: false,
            show_tags: false,
            interval: undefined
        };
    }

    // загружаем данные и запускаем таймер
    async componentDidMount() {

        await this.loadNote();

        await this.startTimer();
    }

    async startTimer() {
        let interval = setInterval(async () => {
            
            await this.update();
            
            console.log("timer")
            
        }, 3000);

        this.setState({interval: interval});
    }

    stopTimer() {
        clearInterval(this.state.interval);
    }

    // останавливаем таймер и принудительно обновляем данные
    async componentWillUnmount() {

        this.stopTimer();

        await this.update();
    }

    async showDialogDelete() {
        this.setState({show_dialog_delete: true})

        await this.loadNote()
    }

    async closeDialogDelete() {
        this.setState({show_dialog_delete: false})

        await this.loadNote()
    }

    showDialogShare() {
        this.setState({show_dialog_share: true})
    }

    async closeDialogShare() {
        this.setState({show_dialog_share: false})

        await this.loadNote()
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
        let note = await NoteApi.getNote(this.props.id);

        // todo convert note to blank

        if (note !== null) {
            // save in state
            this.setState({noteView: note})

            // save blank
            let blank = NoteBuilder.convertViewToBlank(note);

            this.setState({noteBlank: blank})
            
            // editable text
            let text = RichTextEditor.createValueFromString(note.text, "html");

            // save text in html mode in state
            this.setState({rteValue: text})
        }
    }

    // update note
    async update() {
        if(this.state.noteBlank != undefined){
            await NoteApi.updateNote(this.props.id, this.state.noteBlank)
        }
    }

    // set text to blank and rte
    onChange = async (value: EditorValue) => {

        if (this.state.noteBlank != undefined) {
            this.setState({
                noteBlank: {
                    ...this.state.noteBlank,
                    text: value.toString("html")
                }
            })

            // rendered value
            this.setState({rteValue: value})
        }
    }

    onChangeName = async (value: string) => {

        this.setState({
            noteBlank: {
                ...this.state.noteBlank,
                header: value
            }
        })
    }

    render() {
        return ((this.state.noteBlank && this.state.noteView) && (<div>

                    {/*command bar*/}
                    <div className="buttons">

                        <label style={{margin: "5px", fontSize: "24px"}}> Заголовок:</label>

                        <input className="form-control"
                               style={{width: "10rem", margin: "5px"}}
                               value={this.state.noteBlank.header}
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
                                onClick={() => this.props.onClose()}>
                            Close
                        </button>

                    </div>

                    {/*text editor*/}
                    <RichTextEditor editorClassName="editor"
                                    toolbarClassName="editor"
                                    placeholder="Начните ввод..."
                                    value={this.state.rteValue}
                                    onChange={this.onChange}/>

                    {/*dialogs*/}

                    {this.state.show_dialog_delete &&

                        <DeleteNoteDialog show={this.state.show_dialog_delete}
                                          noteView={this.state.noteView}
                                          onClose={() => this.props.onClose()}
                                          onCloseDialog={async () => await this.closeDialogDelete()}/>

                    }

                    {this.state.show_dialog_share &&

                        <ShareNoteDialog show={this.state.show_dialog_share}
                                         noteView={this.state.noteView}
                                         onCloseDialog={async () => await this.closeDialogShare()}/>
                    }

                    {this.state.show_tags &&

                        <TagDialog show={this.state.show_tags}
                                   noteBlank={this.state.noteBlank}
                                   noteView={this.state.noteView}
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
            )
        );
    }
}
