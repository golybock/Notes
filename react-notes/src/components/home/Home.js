import React from "react";
import "./Home.css"
import NoteApi from "../../api/note/NoteApi";
import TagApi from "../../api/note/tag/TagApi";
import Cat from "../../cat.webp";
import Note from "../note/Note";
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';

export default class Home extends React.Component {

    async componentDidMount() {

        this.setState({notes: await NoteApi.getNotes()})

        this.setState({tags: await TagApi.getTags()})
    }

    constructor(props) {
        super(props);
        this.state = {
            tags: [],
            notes: [],
            card_opened : false,
            opened_card_guid : ""
        };
    }

    onClose =() =>{
        this.setState({card_opened : false})
        this.setState({opened_card_guid : null})
    }

    render() {
        return (
            <div className="background">

                {!this.state.card_opened && (
                    <div className="cards">

                        {this.state.notes
                            .map(note =>
                                <Card style={{ width: '16rem' }} key={note.guid}>
                                    <Card.Img variant="top" src={Cat} />
                                    <Card.Body>
                                        <Card.Title style={{color: "black"}}>{note.header}</Card.Title>
                                        <Card.Text style={{color: "black"}} className="">
                                            {note.text}
                                        </Card.Text>
                                        <Button className="" variant="primary" onClick={
                                            () => {
                                                this.setState({card_opened : true})
                                                this.setState({opened_card_guid : note.guid})
                                            }
                                        }>Open</Button>
                                    </Card.Body>
                                </Card>)
                        }

                    </div>
                )}

                {this.state.card_opened && (<Note guid={this.state.opened_card_guid} onClose={this.onClose}></Note>)}

            </div>
        );
    }
}
