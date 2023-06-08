import React from "react";
import "./Home.css"
import NoteApi from "../../api/note/NoteApi";
import Cat from "../../cat.webp";
import Note from "../note/Note";
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';


export default class Home extends React.Component {

    async componentDidMount() {
        await this.loadNotes();
    }

    async loadNotes() {

        let notes = await NoteApi.getNotes();

        let sharedNotes = await NoteApi.getSharedNotes();

        this.setState({notes: notes})

        this.setState({sharedNotes: sharedNotes})
    }

    constructor(props) {
        super(props);
        this.state = {
            tags: [],
            notes: [],
            shared_notes: [],
            card_opened: false,
            opened_card_guid: ""
        };
    }

    async onClose() {
        this.setState({card_opened: false})
        this.setState({opened_card_guid: "null"})

        await this.loadNotes();
    }

    open(id) {
        this.setState({card_opened: true})

        this.setState({opened_card_guid: id})
    }

    render() {
        return (
            <div className="background">

                {!this.state.card_opened && (
                    <div className="buttons">
                        <button className="btn btn-primary-submit" onClick={() => this.open("null")}>
                            Create
                        </button>
                    </div>
                )}

                {!this.state.card_opened && (
                    <div>

                        <h1 style={{textAlign : "left", marginLeft: '20px', marginTop: '10px'}}>Мои заметки</h1>

                        <div className="cards">

                            {this.state.notes
                                .map(note =>
                                    <Card style={{width: '16rem'}} key={note.id}>
                                        <Card.Img variant="top" src={Cat}/>
                                        <Card.Title style={{color: "black", margin: 5}}>
                                            {note.header}
                                        </Card.Title>
                                        <Card.Body>
                                            <Card.Text style={{color: "black"}} className="">
                                                {note.text}
                                            </Card.Text>
                                        </Card.Body>
                                        <Card.Footer>
                                            <Button className="" variant="primary" onClick={() => this.open(note.id)}>
                                                Open
                                            </Button>
                                        </Card.Footer>
                                    </Card>)
                            }

                        </div>

                    </div>

                )}

                {!this.state.card_opened && (
                    <div>

                        <h1 style={{textAlign : "left", marginLeft: '20px', marginTop: '10px'}}>Общие заметки</h1>

                        <div className="cards">

                            {this.state.shared_notes
                                .map(note =>
                                    <Card style={{width: '16rem'}} key={note.id}>
                                        <Card.Img variant="top" src={Cat}/>
                                        <Card.Title style={{color: "black", margin: 5}}>
                                            {note.header}
                                        </Card.Title>
                                        <Card.Body>
                                            <Card.Text style={{color: "black"}} className="">
                                                {note.text}
                                            </Card.Text>
                                        </Card.Body>
                                        <Card.Footer>
                                            <Button className="" variant="primary" onClick={() => this.open(note.id)}>
                                                Open
                                            </Button>
                                        </Card.Footer>
                                    </Card>)
                            }

                        </div>

                    </div>

                )}

                {this.state.card_opened && (<Note id={this.state.opened_card_guid} onClose={async () => {
                    await this.onClose()
                }}></Note>)}

            </div>
        );
    }
}
