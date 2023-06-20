import React from "react";
import { Stage, Layer, Rect, Transformer, Image } from 'react-konva';
import URLImage from "./layers/UrlImage";
import Rectangle from "./layers/Rectangle";

export default class ImagesLayer extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            rectangles: this.initialRectangles,
            selected: null
        }
    }

    initialRectangles = [
        {
            x: 10,
            y: 10,
            width: 100,
            height: 100,
            fill: 'red',
            id: 'rect1',
        },
        {
            x: 150,
            y: 150,
            width: 100,
            height: 100,
            fill: 'green',
            id: 'rect2',
        },
    ];

    checkDeselect = (e) => {
        // deselect when clicked on empty area
        const clickedOnEmpty = e.target === e.target.getStage();
        if (clickedOnEmpty) {
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
                    {this.state.rectangles.map((rect, i) => {
                        return (
                            <Rectangle
                                src="https://konvajs.org/assets/yoda.jpg"
                                key={i}
                                shapeProps={rect}
                                isSelected={rect.id === this.state.selected}
                                onSelect={() => {
                                    this.setState({selected: rect.id});
                                }}
                                onChange={(newAttrs) => {
                                    const rects = this.state.rectangles.slice();
                                    rects[i] = newAttrs;
                                    this.setState({rectangles: rects})
                                }}
                            />
                        );
                    })}
                    {/*<URLImage  x={150} />*/}
                </Layer>
            </Stage>
        )
    }

}