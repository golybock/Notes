import React from "react";
import {Image, Rect, Transformer} from "react-konva";

export default class Rectangle extends React.Component {

    constructor(props) {
        super(props);

        this.trRef = React.createRef();
        this.shapeRef = React.createRef();

        this.state = {
            isSelected : this.props.isSelected,
            image: null
        }
    }

    componentDidMount() {
        this.loadImage();

        if (this.state.isSelected) {
            this.trRef.current.nodes([this.shapeRef.current]);
            this.trRef.current.getLayer().batchDraw();
        }
    }

    componentWillUnmount() {
        this.image.removeEventListener('load', this.handleLoad);
    }
    loadImage() {
        this.image = new window.Image();
        this.image.src = this.props.src;
        this.image.addEventListener('load', this.handleLoad);
    }

    handleLoad = () => {
        this.setState({
            image: this.image,
        });
    };
    
    render() {
        return ((
            <React.Fragment>
                <Image
                    image={this.state.image}
                    onClick={() => {
                        this.props.onSelect()
                        this.setState({isSelected: true})
                    }}
                    onTap={() => {
                        this.props.onSelect()
                        this.setState({isSelected: true})
                    }}
                    ref={this.shapeRef}
                    {...this.props.shapeProps}
                    draggable
                    onDragEnd={(e) => {
                        this.props.onChange({
                            ...this.props.shapeProps,
                            x: e.target.x(),
                            y: e.target.y(),
                        });
                    }}
                    onTransformEnd={(e) => {
                        const node = this.shapeRef.current;
                        const scaleX = node.scaleX();
                        const scaleY = node.scaleY();

                        // we will reset it back
                        node.scaleX(1);
                        node.scaleY(1);
                        onChange({
                            ...this.props.shapeProps,
                            x: node.x(),
                            y: node.y(),
                            // set minimal value
                            width: Math.max(5, node.width() * scaleX),
                            height: Math.max(node.height() * scaleY),
                        });
                    }}
                />
                {this.props.isSelected && (
                    <Transformer
                        ref={this.trRef}
                        boundBoxFunc={(oldBox, newBox) => {
                            // limit resize
                            if (newBox.width < 5 || newBox.height < 5) {
                                return oldBox;
                            }
                            return newBox;
                        }}
                    />
                )}
            </React.Fragment>
        ));
    }
};