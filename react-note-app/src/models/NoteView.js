class NoteView {
    constructor(id, header, creationDate, lastEditDate, text, userId, tags) {
        this.id = id;
        this.header = header;
        this.creationDate = creationDate;
        this.lastEditDate = lastEditDate;
        this.text = text;
        this.userId = userId;
        this.tags = tags;
    }
}

export default NoteView