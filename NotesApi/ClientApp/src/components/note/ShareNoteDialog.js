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
            permission_level: 0,
            email : ""
        }
    }

    async share() {
        
        let shareBlank = new ShareNoteBlank(this.props.id, this.state.email, this.state.permission_level)
        
        let share = await NoteApi.shareNote(shareBlank)

        this.props.onCloseDialog()
    }

    render() {
        return (
            <Modal
                show={this.props.show}
                onHide={() => this.props.onCloseDialog()}
                backdrop="static"
                centered>

                <Modal.Header closeButton>
                    <Modal.Title>Share note</Modal.Title>
                </Modal.Header>

                <Modal.Body>

                    <Form>

                        <Form.Group className="mb-3">
                            <Form.Label>Note name</Form.Label>

                            <Form.Control
                                type="email"
                                placeholder="email"
                                autoFocus
                                required
                                value={this.state.email}
                                onChange={(e) => {
                                    this.setState({
                                        email: e.target.value
                                    })
                                }}
                            />

                        </Form.Group>

                    </Form>

                </Modal.Body>

                <Modal.Footer>

                    <Button variant="secondary"
                            className="btn"
                            onClick={() => this.props.onCloseDialog()}>
                        Close
                    </Button>

                    <Button variant="primary"
                            className="btn btn-primary-note"
                            onClick={this.share}>
                        Share
                    </Button>

                </Modal.Footer>

            </Modal>
        )
    }

}