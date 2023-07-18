import React from "react";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import TagApi from "../../../api/note/tag/TagApi";
import AsyncSelect from "react-select/async";
import INoteView from "../../../models/view/note/INoteView";
import {Guid} from "guid-typescript";
import ITagView from "../../../models/view/note/ITagView";
import INoteBlank from "../../../models/blank/note/INoteBlank";
import {MultiValue} from "react-select";

interface IProps {
    show: boolean;
    noteView: INoteView;
    noteBlank : INoteBlank;
    update: Function;
    onCloseDialog: Function;
}

interface IState {
    selected: MultiValue<{label : string, value : Guid}>;
    default_tags: Array<ITagView>;
}

export default class TagDialog extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);

        this.state = {
            selected: [],
            default_tags: this.props.noteView.tags
        }
    }

    componentDidMount() {
        this.setSelectedTags()
    }

    setSelectedTags() {
        let tags = this.props.noteView.tags;

        let arr: Array<{ label: string, value: Guid }> = []

        tags.forEach(element => {
            let dropDownEle = {label: element.name, value: element.id};
            arr.push(dropDownEle);
        });

        this.setState({selected: arr})
    }

    async getTags() {

        let tags = await TagApi.getTags()

        let arr: Array<{ label: string, value: Guid }> = []

        tags.forEach(element => {
            let dropDownEle = {label: element.name, value: element.id};
            arr.push(dropDownEle);
        });

        return arr;
    }

    async tagChosen(e : MultiValue<{label : string, value : Guid}>) {
        let arr : Array<Guid> = []

        e.forEach(element => {
            arr.push(element.value);
        });

        this.setState({selected: e})

        this.props.noteBlank.tags = arr;
    }

    async updateTags() {
        await this.props.update()
        await this.props.onCloseDialog()
    }

    render() {
        return (
            <Modal
                show={this.props.show}
                onHide={() => this.props.onCloseDialog()}
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
                                onChange={async (e : MultiValue<{label : string, value : Guid}>) => await this.tagChosen(e)}
                            />

                        </Form.Group>

                    </Form>

                </Modal.Body>

                <Modal.Footer>

                    <Button className="btn"
                            variant="secondary"
                            onClick={() => this.props.onCloseDialog()}>
                        Close
                    </Button>

                    <Button className="btn btn-primary-note"
                            onClick={async () => await this.updateTags()}>
                        Save Changes
                    </Button>

                </Modal.Footer>

            </Modal>
        )
    }

}