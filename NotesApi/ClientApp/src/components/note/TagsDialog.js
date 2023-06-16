import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import TagApi from "../../api/note/tag/TagApi";
import AsyncSelect from "react-select/async";

export default class TagDialog extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            selected: []
        }
    }

    componentDidMount() {
        this.setSelectedTags()
    }

    setSelectedTags() {
        let tags = this.props.note.tags;

        let arr = []

        tags.forEach(element => {
            let dropDownEle = {label: element.name, value: element.id};
            arr.push(dropDownEle);
        });
        
        this.setState({selected: arr})
    }

    async getTags() {
        
        let tags = await TagApi.getTags()

        let arr = []

        tags.forEach(element => {
            let dropDownEle = {label: element.name, value: element.id};
            arr.push(dropDownEle);
        });

        return arr;
    }

    async tagChosen(e) {
        let arr = []

        e.forEach(element => {
            let dropDownEle = {name: element.label, id: element.value};
            arr.push(dropDownEle);
        });
        
        this.setState({selected : e})
        
        this.props.note.tags = arr;
        
        // save in note
        await this.props.update()
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
                                value={this.state.selected}
                                loadOptions={this.getTags}
                                onChange={(e) => this.tagChosen(e)}
                            />

                        </Form.Group>

                    </Form>

                </Modal.Body>

                <Modal.Footer>

                    <Button className="btn"
                            variant="secondary"
                            onClick={this.props.onCloseDialog}>
                        Close
                    </Button>

                    <Button className="btn btn-primary-note">
                        Save Changes
                    </Button>

                </Modal.Footer>

            </Modal>
        )
    }

}