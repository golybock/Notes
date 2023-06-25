import React from "react";
import "./Home.css"
import NoteApi from "../../api/note/NoteApi";
import Note from "../note/Note";
import NoteCardsList from "../note/NoteCardsList";
import CreateNoteDialog from "../note/dialogs/CreateNoteDialog";

export default class Home extends React.Component {
    
    constructor(props) {
        super(props);
        this.state = {
            notes: [],
            shared_notes: [],
            card_opened: false,
            opened_card_guid: "",
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
        this.setState({opened_card_guid: ""})

        await this.loadNotes();
    }

    open(id) {
        this.setState({card_opened: true})

        this.setState({opened_card_guid: id})
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
                                   open={(id) => this.open(id)}
                                   notes={this.state.notes}/>

                )}

                {/*render only if shared notes exists*/}
                {(!this.state.card_opened && this.state.shared_notes.length !== 0)&& (

                    <NoteCardsList header="Общие заметки"
                                   open={(id) => this.open(id)}
                                   notes={this.state.shared_notes}/>

                )}

                {/*dont work if onClose = this.onClose*/}
                {this.state.card_opened && (

                    <Note id={this.state.opened_card_guid}
                          onClose={async () => await this.onClose()}/>

                )}

                {/*create note modal*/}
                <CreateNoteDialog
                    show={this.state.show}
                    handleClose={() => this.handleClose()}
                    open={(id) => this.open(id)}/>

            </div>
        );
    }
}
