import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import TagApi from "../../api/note/tag/TagApi";
import AsyncSelect from "react-select/async";
import {Await} from "react-router";

export default class TagDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            note_tags: [],
            selected: []
        }
    }

    async componentDidMount() {
        // this.setSelectedTags()
    }

    // setSelectedTags() {
    //     let tags = this.props.note.tags;
    //
    //     let arr = []
    //
    //     tags.forEach(element => {
    //         let dropDownEle = {label: element.name, value: element.id};
    //         arr.push(dropDownEle);
    //     });
    //
    //     this.setState({note_tags: arr})
    // }

    async getTags() {
        let tags = await TagApi.getTags()

        let arr = []

        tags.forEach(element => {
            let dropDownEle = {label: element.name, value: element.id};
            arr.push(dropDownEle);
        });

        return arr;
    }

    tagChosen(e) {
        // save in note

        this.setState({selected: e.value})
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

                            <AsyncSelect
                                noOptionsMessage={() => "tags not found"}
                                cacheOptions
                                isMulti
                                defaultOptions
                                loadOptions={async () => await this.getTags()}
                                onChange={(e) => this.tagChosen(e)}
                            />

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