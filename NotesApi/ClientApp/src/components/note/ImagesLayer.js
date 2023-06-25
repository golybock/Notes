import React from "react";
import {Stage, Layer} from 'react-konva';
import Rectangle from "./layers/Rectangle";

export default class ImagesLayer extends React.Component {

    constructor(props) {
        super(props);

        // load images to server
        document.onpaste = (evt) => {
            const dt = evt.clipboardData || window.clipboardData;

            if (dt !== undefined) {
                const file = dt.files[0];
                console.log(file);
            }

        };

        this.state = {
            images: this.props.images,
            selected: null
        }
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