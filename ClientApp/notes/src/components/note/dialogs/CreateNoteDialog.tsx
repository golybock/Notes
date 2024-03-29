import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import NoteApi from "../../../api/note/NoteApi";
import INoteBlank from "../../../models/blank/note/INoteBlank";

interface IProps{
    open : Function;
    handleClose : Function;
    show : boolean;
}

interface IState{
    name: string;
}

export default class CreateNoteDialog extends React.Component<IProps, IState> {
    constructor(props : IProps) {
        super(props);
        this.state = {
            name: ""
        }
    }

    async create() {

        let noteBlank : INoteBlank = {
            header : this.state.name,
        }

        let id = await NoteApi.createNote(noteBlank)

        this.setState({name: ""})

        this.props.open(id)

        this.props.handleClose()
    }

    render() {
        return (<Modal
            show={this.props.show}
            onHide={() => this.props.handleClose()}
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

                <button className="btn btn-secondary"
                        onClick={() => this.props.handleClose()}>
                    Close
                </button>

                <button className="btn btn-primary"
                        onClick={async () => await this.create()}>
                    Save Changes
                </button>

            </Modal.Footer>

        </Modal>)
    }

}