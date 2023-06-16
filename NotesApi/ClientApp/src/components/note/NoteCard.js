import React from "react";
import Card from "react-bootstrap/Card";
import Cat from "../../resources/cat.webp";
import Button from "react-bootstrap/Button";

export default class NoteCard extends React.Component {

    render() {
        return (
            <Card style={{width: '16rem'}}
                  key={this.props.note.id}
                  onClick={() => this.props.open(this.props.note.id)}>

                <Card.Img variant="top"
                          style={{padding: "15px"}}
                          src={Cat}/>

                <Card.Title style={{color: "black", margin: 5}}>
                    {this.props.note.header}
                </Card.Title>

                <Card.Body>

                    <Card.Text style={{color: "black"}}>
                        {this.props.note.text}
                    </Card.Text>

                </Card.Body>

            </Card>
        )
    }

}