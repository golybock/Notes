import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import NoteApi from "../../api/note/NoteApi";
import {NoteBlank} from "../../models/blank/note/NoteBlank";

export default class ShareNoteDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            id : "",
            permission_level: 0,
            email : ""
        }
    }

    async share() {
        
        let shareBlank = new ShareNoteBlank(this.state.id, this.state.email, this.state.permission_level)
        
        let share = await NoteApi.shareNote(shareBlank)

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