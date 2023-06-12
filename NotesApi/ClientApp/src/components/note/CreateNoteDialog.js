import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import NoteApi from "../../api/note/NoteApi";
import {NoteBlank} from "../../models/blank/note/NoteBlank";

export default class CreateNoteDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            name: ""
        }
    }

    async create() {
        
        let noteBlank = new NoteBlank(this.state.name, "");
        
        let id = await NoteApi.createNote(noteBlank)

        this.setState({name : ""})
        
        this.props.open(id)

        this.props.handleClose()
    }

    render() {
        return (
            <Modal
                show={this.props.show}
                onHide={this.props.handleClose}
                backdrop="static"
                centered>

                <Modal.Header closeButton>
                    <Modal.Title>Create note</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    
                    <Form>
                        
                        <Form.Group className="mb-3">
                            <Form.Label>Note name</Form.Label>
                            
                            <Form.Control
                                type="text"
                                placeholder="name"
                                autoFocus
                                required
                                value={this.state.name}
                                onChange={(e) => {
                                    this.setState({
                                        name: e.target.value
                                    })
                                }}
                            />
                            
                        </Form.Group>
                        
                    </Form>
                    
                </Modal.Body>

                <Modal.Footer>

                    <Button variant="secondary" 
                            className="btn"
                             onClick={this.props.handleClose}>
                        Close
                    </Button>

                    <Button variant="primary"
                            className="btn btn-primary-note"
                            onClick={async () => await this.create()}>
                        Save Changes
                    </Button>

                </Modal.Footer>

            </Modal>
        )
    }

}