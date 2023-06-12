import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import TagApi from "../../api/note/tag/TagApi";

export default class TagDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            tags: [],
            note_tags: []
        }
    }

    async componentDidMount() {
        await this.getTags()
    }

    async getTags() {
        let tags = await TagApi.getTags()

        this.setState({tags : tags})
    }

    render() {
        return (
            <Modal
                show={this.props.show}
                onHide={this.props.onCloseDialog}
                backdrop="static"
                centered>

                <Modal.Header closeButton>
                    <Modal.Title>Note tags</Modal.Title>
                </Modal.Header>

                <Modal.Body>

                    <Form>

                        <Form.Group className="mb-3">
                            <Form.Label>Note name</Form.Label>

                            {/*<Form.Control*/}
                            {/*    type="text"*/}
                            {/*    placeholder="name"*/}
                            {/*    autoFocus*/}
                            {/*    required*/}
                            {/*    value={this.state.name}*/}
                            {/*    onChange={(e) => {*/}
                            {/*        this.setState({*/}
                            {/*            name: e.target.value*/}
                            {/*        })*/}
                            {/*    }}*/}
                            {/*/>*/}

                        </Form.Group>

                    </Form>

                </Modal.Body>

                <Modal.Footer>

                    <Button variant="secondary"
                            className="btn"
                            onClick={this.props.onCloseDialog}>
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