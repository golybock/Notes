import React from "react";
import Modal from "react-bootstrap/Modal";
import NoteApi from "../../../api/note/NoteApi";
import {FormLabel} from "react-bootstrap";

export default class DeleteNoteDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            name: ""
        }
    }

    async delete() {
        await NoteApi.deleteNote(this.props.note.id)

        this.props.onClose()
    }

    render() {
        return (
            <Modal
                show={this.props.show}
                onHide={this.props.onCloseDialog}
                backdrop="static"
                centered>

                <Modal.Header closeButton>
                    <Modal.Title>Удаление</Modal.Title>
                </Modal.Header>

                <Modal.Body>

                    <FormLabel>Удалить {this.props.note.name}?</FormLabel>

                </Modal.Body>

                <Modal.Footer>

                    <button onClick={this.props.onCloseDialog}
                            className="btn btn-secondary">
                        Нет
                    </button>

                    <button onClick={async () => await this.delete()}
                            className="btn btn-danger">
                        Да, удалить
                    </button>

                </Modal.Footer>

            </Modal>
        )
    }

}