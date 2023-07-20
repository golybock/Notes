import React from "react";
import "./Home.css"
import NoteApi from "../../api/note/NoteApi";
import Note from "../note/Note";
import NoteCardsList from "../note/NoteCardsList";
import CreateNoteDialog from "../note/dialogs/CreateNoteDialog";
import INoteView from "../../models/view/note/INoteView";
import {Guid} from "guid-typescript";

interface IProps {

}

interface IState {
    notes: Array<INoteView>;
    shared_notes: Array<INoteView>;
    opened_card_id?: Guid;
    show: boolean;
    interval?: NodeJS.Timer;
}

export default class Home extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);
        this.state = {
            notes: [],
            shared_notes: [],
            opened_card_id: undefined,
            show: false,
            interval: undefined
        };
    }

    async startTimer() {
        let interval = setInterval(async () => {

            await this.loadNotes();

            console.log("timer")

        }, 3000);

        this.setState({interval: interval});
    }

    stopTimer() {
        clearInterval(this.state.interval);
    }

    async componentDidMount() {
        await this.loadNotes();

        await this.startTimer();
    }

    async componentWillUnmount() {
        this.stopTimer();
    }

    async loadNotes() {

        let notes = await NoteApi.getNotes();

        let sharedNotes = await NoteApi.getSharedNotes();

        this.setState({notes: notes})

        this.setState({shared_notes: sharedNotes})
    }

    async onClose() {

        this.setState({opened_card_id: undefined})

        await this.loadNotes();
    }

    open(id: Guid) {

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

                {!this.state.opened_card_id && (
                    <div className="buttons">
                        <button className="btn btn-primary-submit"
                                onClick={() => this.handleOpen()}>
                            Create
                        </button>
                    </div>
                )}

                {/*render only if notes exists*/}
                {(!this.state.opened_card_id && this.state.notes.length !== 0) && (

                    <NoteCardsList header="Заметки"
                                   open={(id: Guid) => this.open(id)}
                                   notes={this.state.notes}/>

                )}

                {/*render only if shared notes exists*/}
                {(!this.state.opened_card_id && this.state.shared_notes.length !== 0) && (

                    <NoteCardsList header="Общие заметки"
                                   open={(id: Guid) => this.open(id)}
                                   notes={this.state.shared_notes}/>

                )}

                {/*dont work if onClose = this.onClose*/}
                {this.state.opened_card_id && (

                    <Note id={this.state.opened_card_id}
                          onClose={async () => await this.onClose()}/>

                )}

                {/*create note modal*/}
                <CreateNoteDialog
                    show={this.state.show}
                    handleClose={() => this.handleClose()}
                    open={(id: Guid) => this.open(id)}/>

            </div>
        );
    }
}
