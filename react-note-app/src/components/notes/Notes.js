import React from "react";
import noteApi from "../../api/note/NoteApi";
import tagApi from "../../api/note/tag/TagApi";

class Notes extends React.Component{

    async componentDidMount() {

        this.setState({notes: await noteApi.getNotes()})

        this.setState({tags: await tagApi.getTags()})
    }

    constructor(props) {
        super(props);
        this.state = {
            tags: [],
            notes: []
        };
    }

    render() {
        return (
            <div className="App">

                <header className="App-header">

                    <h1>Tags</h1>

                    <ul>
                        {
                            this.state.tags
                                .map(tag =>
                                    <li key={tag.id}>{tag.name}</li>
                                )
                        }
                    </ul>

                    <h1>Notes</h1>

                    <ul>
                        {
                            this.state.notes
                                .map(note =>
                                    <div>
                                        <li key={note["guid"]}>{note["header"]}</li>

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

export default Notes;
