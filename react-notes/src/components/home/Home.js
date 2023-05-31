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
        // let tags = await TagApi.getTags();
        //
        // this.setState({tags: tags})
    }

    async loadNotes() {
        let notes = await NoteApi.getNotes();

        this.setState({notes: notes})
    }

    constructor(props) {
        super(props);
        this.state = {
            tags: [],
            notes: [],
            card_opened: false,
            opened_card_guid: ""
        };
    }

    async onClose() {
        this.setState({card_opened: false})
        this.setState({opened_card_guid: null})

        await this.loadNotes();
    }

    open(note) {
        this.setState({card_opened: true})
        this.setState({opened_card_guid: note.guid})
    }

    render() {
        return (
            <div className="background">

                {!this.state.card_opened && (
                    <div className="cards">

                        {this.state.notes
                            .map(note =>
                                <Card style={{width: '16rem'}} key={note.guid}>
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
                                        <Button className="" variant="primary" onClick={() => this.open(note)}>
                                            Open
                                        </Button>
                                    </Card.Footer>
                                </Card>)
                        }

                    </div>
                )}

                {this.state.card_opened && (<Note guid={this.state.opened_card_guid} onClose={async () => {
                    await this.onClose()
                }}></Note>)}

            </div>
        );
    }
}
