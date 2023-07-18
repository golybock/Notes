import React from "react";
import Card from "react-bootstrap/Card";
import Cat from "../../resources/cat.webp";
import INoteView from "../../models/view/note/INoteView";

interface IProps{
    note : INoteView;
    open : Function;
}

interface IState{
    
}

export default class NoteCard extends React.Component<IProps, IState> {

    render() {
        return (
            <Card style={{width: '16rem'}}
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