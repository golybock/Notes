namespace NotesApi.Services.Note;

public static class NoteFileManager
{
    /// <summary>
    /// read text from source
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static async Task<string?> GetNoteText(string source)
    {
        string path = "Files/" + source;

        if (!File.Exists(path))
            return null;

        using StreamReader sr = new StreamReader(path);

        return await sr.ReadToEndAsync();
    }

    /// <summary>
    /// Write text into file by source 
    /// </summary>
    /// <param name="fileName">filName </param>
    /// <param name="text">note text</param>
    public static async Task UpdateNoteText(string fileName, string text)
    {
        string source = $"Files/{fileName}";

        await using StreamWriter sw = new StreamWriter(source);

        await sw.WriteLineAsync(text);
    }

    /// <summary>
    /// Generate path file, write into file text and returns filepath
    /// </summary>
    /// <param name="text">note text</param>
    /// <returns>path to file</returns>
    public static async Task<string> CreateNoteText(string text)
    {
        string fileName = $"{Guid.NewGuid()}.txt";

        string source = $"Files/{fileName}";

        await using StreamWriter sw = new StreamWriter(source);

        await sw.WriteLineAsync(text);

        return fileName;
    }
}