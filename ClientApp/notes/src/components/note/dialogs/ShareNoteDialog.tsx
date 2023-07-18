import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import NoteApi from "../../../api/note/NoteApi";
import IShareBlank from "../../../models/blank/note/share/IShareBlank";
import IUserView from "../../../models/view/user/IUserView";
import INoteView from "../../../models/view/note/INoteView";

interface IProps {
    show : boolean;
    onCloseDialog: Function;
    noteView: INoteView;
}

interface IState {
    users: Array<IUserView>;
    permission_level: number;
    email: string;
    error: string;
}

export default class ShareNoteDialog extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);

        this.state = {
            users: [],
            permission_level: 1,
            email: "",
            error: ""
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

        let shareBlank: IShareBlank = {
            noteId: this.props.noteView.id,
            email: this.state.email,
            permissionLevel: this.state.permission_level
        }

        let shared = await NoteApi.shareNote(shareBlank)

        if (shared) {
            this.props.onCloseDialog()
        } else {
            this.setState({error: "Пользователь не найден"})
        }
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
                            <Form.Label>Email пользователя: </Form.Label>

                            {this.state.error.length > 0 &&
                                <label style={{color: "red", margin: "5px"}}>{this.state.error}</label>
                            }

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

                            {/*<ul>*/}
                            {/*    {this.state.users.map(u => (*/}
                            {/*        <li className="Card-item">*/}
                            {/*            <div className="Card-header">*/}
                            {/*                <h4>{u.email}</h4>*/}
                            {/*            </div>*/}
                            {/*        </li>*/}
                            {/*    ))}*/}
                            {/*</ul>*/}

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