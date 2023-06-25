import React from "react";
import {Stage, Layer} from 'react-konva';
import NoteApi from "../../api/note/NoteApi";
import Rectangle from "./layers/Rectangle";
import Note from "./Note";
import ImageNoteBlank from "../../models/blank/note/ImageNoteBlank";

export default class ImagesLayer extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            images: this.props.images,
            selected: null
        }
    }

    async componentDidMount() {
        // load images to server
        document.onpaste = async (evt) => {
            const dt = evt.clipboardData || window.clipboardData;

            if (dt !== undefined) {
                const file = dt.files[0];

                let id = await NoteApi.uploadFile(file)

                this.props.note.images.push(new ImageNoteBlank(id, 150, 150, 150, 150, null))

                await this.props.uploadImage();

                console.log(id)
            }

        };
    }

    checkDeselect = (e) => {
        // deselect when clicked on empty area
        try {
            const clickedOnEmpty = e.target === e.target.getStage();
            if (clickedOnEmpty) {
                this.setState({selected: null});
            }
        } catch {
            this.setState({selected: null});
        }
    };

    render() {
        return (
            <Stage
                width={window.innerWidth}
                height={window.innerHeight}
                onMouseDown={() => this.checkDeselect()}
                onTouchStart={() => this.checkDeselect()}
            >
                <Layer>
                    {this.state.images.map((img) => {
                        return (
                            <Rectangle
                                src={img.url}
                                key={img.id}
                                shapeProps={img}
                                onSelect={() => {
                                    this.setState({selected: img.id});
                                }}
                                onChange={(newAttrs) => {
                                    const rects = this.state.images.slice();
                                    rects[img.id] = newAttrs;
                                    this.setState({images: rects})
                                }}
                            />
                        );
                    })}
                </Layer>
            </Stage>
        )
    }

}