import logo from './logo.svg';
import './App.css';
import React from "react";
import tagApi from "./api/note/tag/TagApi";
import noteApi from "./api/note/NoteApi";

class MainApp extends React.Component {

    async componentDidMount() {

        this.setState({notes: await noteApi.getNotes()})

        this.setState({tags: await tagApi.getTags()})
    }

    constructor(props) {
        super(props);
        this.state = {
            tags: [],
            notes : []
        };
    }

    render() {
        return (
            <div className="App">
                <header className="App-header">
                    <ul>
                        {
                            this.state.tags
                                .map(tag =>
                                    <li key={tag.id}>{tag.name}</li>
                                )
                        }
                    </ul>

                    <ul>
                        {
                            this.state.notes
                                .map(note =>
                                    <div>
                                        <li key={note.guid}>{note.header}</li>

                                        <ul>
                                            {
                                                note.tags
                                                    .map(tag =>
                                                        <div>

                                                            <li key={tag.id}>{tag.name}</li>
                                                        </div>
                                                    )
                                            }
                                        </ul>

                                    </div>
                                )
                        }
                    </ul>
                </header>
            </div>
        );
    }
}

export default MainApp;
