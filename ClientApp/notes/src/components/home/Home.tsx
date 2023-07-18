import React from "react";
import "./Home.css"
import NoteApi from "../../api/note/NoteApi";
import Note from "../note/Note";
import NoteCardsList from "../note/NoteCardsList";
import CreateNoteDialog from "../note/dialogs/CreateNoteDialog";
import INoteView from "../../models/view/note/INoteView";
import {Guid} from "guid-typescript";

interface IProps{
    
}

interface IState {
    notes: Array<INoteView>;
    shared_notes: Array<INoteView>;
    card_opened: boolean;
    opened_card_id?: Guid;
    show: boolean;
}

export default class Home extends React.Component<IProps, IState> {
    
    constructor(props: IProps) {
        super(props);
        this.state = {
            notes: [],
            shared_notes: [],
            card_opened: false,
            opened_card_id: undefined,
            show: false,
        };
    }

    async componentDidMount() {
        await this.loadNotes();
    }
    
    async loadNotes() {

        let notes = await NoteApi.getNotes();

        let sharedNotes = await NoteApi.getSharedNotes();

        this.setState({notes: notes})

        this.setState({shared_notes: sharedNotes})
    }
    
    async onClose() {

        this.setState({card_opened: false})
        this.setState({opened_card_id: undefined})

        await this.loadNotes();
    }

    open(id: Guid) {
        this.setState({card_opened: true})

        this.setState({opened_card_id: id})
    }

    handleClose() {
        this.setState({show: false})
    }

    handleOpen() {
        this.setState({show: true})
    }

    render() {
        return (
            <div className="background">

                {!this.state.card_opened && (
                    <div className="buttons">
                        <button className="btn btn-primary-submit"
                                onClick={() => this.handleOpen()}>
                            Create
                        </button>
                    </div>
                )}

                {/*render only if notes exists*/}
                {(!this.state.card_opened && this.state.notes.length !== 0)&& (

                    <NoteCardsList header="Заметки"
                                   open={(id : Guid) => this.open(id)}
                                   notes={this.state.notes}/>

                )}

                {/*render only if shared notes exists*/}
                {(!this.state.card_opened && this.state.shared_notes.length !== 0)&& (

                    <NoteCardsList header="Общие заметки"
                                   open={(id : Guid) => this.open(id)}
                                   notes={this.state.shared_notes}/>

                )}

                {/*dont work if onClose = this.onClose*/}
                {this.state.card_opened && (

                    <Note id={this.state.opened_card_id}
                          onClose={async () => await this.onClose()}/>

                )}

                {/*create note modal*/}
                <CreateNoteDialog
                    show={this.state.show}
                    handleClose={() => this.handleClose()}
                    open={(id : Guid) => this.open(id)}/>

            </div>
        );
    }
}
