namespace NotesApi.Services.Note.NoteFileManager;

public interface INoteFileManager
{
    /// <summary>
    /// Write text into file by source (if file not exists, it can be created) 
    /// </summary>
    /// <param name="noteId">filName </param>
    /// <param name="text">note text</param>
    public Task SetNoteText(Guid noteId, string text);

    /// <summary>
    /// read text from source
    /// </summary>
    /// <param name="noteId"></param>
    /// <returns></returns>
    public Task<string?> GetNoteText(Guid noteId);
}