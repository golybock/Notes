class NoteApi {
    // получения списка продуктов
    static getNotes(){
        return fetch("https://localhost:7058/api/Note/Notes", {
            headers: {
                Accept: "*/*"
            }
        });
    }
}