import React from "react";
import Modal from "react-bootstrap/Modal";
import Button from "react-bootstrap/Button";
import NoteApi from "../../api/note/NoteApi";
import {FormLabel} from "react-bootstrap";

export default class DeleteNoteDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            name: ""
        }
    }

    async delete() {
        await NoteApi.deleteNote(this.props.id)
        
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

                    <FormLabel>Удалить {this.props.name}?</FormLabel>

                </Modal.Body>

                <Modal.Footer>

                    <Button variant="secondary" onClick={this.props.onCloseDialog}>
                        Нет
                    </Button>

                    <Button variant="danger"
                            onClick={async () => await this.delete()}>
                        Да, удалить
                    </Button>

                </Modal.Footer>

            </Modal>
        )
    }

}