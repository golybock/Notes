import React from "react";
import noteApi from "../../api/note/NoteApi";
import tagApi from "../../api/note/TagApi";
import {TagView} from "../../models/view/note/TagView";
import {NoteView} from "../../models/view/note/NoteView";


export class Notes extends React.Component{

    private notes : NoteView[] = []
    private tags : TagView[] = []

    async componentDidMount() {

        this.notes = await noteApi.getNotes()

        this.tags = await tagApi.getTags()
    }


    render() {
        return (
            <div className="App">

                <header className="App-header">

                    <h1>Tags</h1>

                    <ul>
                        {
                            this.tags
                                .map((tag : TagView) =>
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
                                                    .map((tag : TagView)  => {
                                                            return <div>

                                                                <li key={tag.id}>{tag.name}</li>
                                                            </div>;
                                                        }
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
