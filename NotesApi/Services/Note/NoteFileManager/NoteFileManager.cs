namespace NotesApi.Services.Note.NoteFileManager;

public class NoteFileManager : INoteFileManager
{
    private static readonly string TextLayerFormat = ".html";
    
    /// <summary>
    /// read text from source
    /// </summary>
    /// <param name="noteId"></param>
    /// <returns></returns>
    public async Task<string?> GetNoteText(Guid noteId)
    {
        string path = "Files/" + noteId + TextLayerFormat;

        if (!File.Exists(path))
            return null;

        using StreamReader sr = new StreamReader(path);

        return await sr.ReadToEndAsync();
    }

    /// <summary>
    /// Write text into file by source (if file not exists, it can be created) 
    /// </summary>
    /// <param name="noteId">filName </param>
    /// <param name="text">note text</param>
    public async Task SetNoteText(Guid noteId, string text)
    {
        string source = $"Files/{noteId}" + TextLayerFormat;

        await using StreamWriter sw = new StreamWriter(source);

        await sw.WriteLineAsync(text);
    }
}