using System.Text.Json;

namespace NotesApi.Services.Note;

public static class NoteFileManager
{
    private static readonly string TextLayerFormat = ".html";
    
    /// <summary>
    /// read text from source
    /// </summary>
    /// <param name="noteId"></param>
    /// <returns></returns>
    public static async Task<string?> GetNoteText(Guid noteId)
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
    public static async Task UpdateNoteText(Guid noteId, string text)
    {
        string source = $"Files/{noteId}" + TextLayerFormat;

        await using StreamWriter sw = new StreamWriter(source);

        await sw.WriteLineAsync(text);
    }

    /// <summary>
    /// Generate path file, write into file text and returns filepath
    /// </summary>
    /// <param name="noteId">note id</param>
    /// <param name="text">note text</param>
    /// <returns>path to file</returns>
    public static async Task CreateNoteText(Guid noteId, string text)
    {
        string fileName = $"{noteId}" + TextLayerFormat;

        string source = $"Files/{fileName}";

        await using StreamWriter sw = new StreamWriter(source);

        await sw.WriteLineAsync(text);
    }

    /// <summary>
    /// Create note images layer
    /// </summary>
    /// <returns>source</returns>
    public static async Task CreateNoteFiles(Guid noteId)
    {
        string fileName = $"{noteId}" + TextLayerFormat;

        string source = $"Files/{fileName}";

        await using var stream = File.Create(source);
        stream.Close();
    }
}