import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import NoteApi from "../../api/note/NoteApi";
import ShareNoteBlank from "../../models/blank/note/ShareNoteBlank";

export default class ShareNoteDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            users : [],
            permission_level: 1,
            email: ""
        }
    }
    
    async componentDidMount() {
        // await this.GetShared()
    }

    // async GetShared(){
    //     let note = await NoteApi.getNote(this.props.id);
    //    
    //     this.setState({users: note.sharedUsers})
    // }
    
    async share() {

        let shareBlank = new ShareNoteBlank(this.props.id,
            this.state.email,
            this.state.permission_level)

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

                            <ul>
                                {this.state.users.map(u => (
                                    <li className="Card-item">
                                        <div className="Card-header">
                                            <h4>{u.email}</h4>
                                        </div>
                                    </li>
                                ))}
                            </ul>

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
                            onClick={async () => await this.share()}>
                        Share
                    </Button>

                </Modal.Footer>

            </Modal>
        )
    }

}