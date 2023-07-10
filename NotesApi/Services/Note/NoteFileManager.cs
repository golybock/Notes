using System.Text.Json;

namespace NotesApi.Services.Note;

public static class NoteFileManager
{
    private static readonly string TextLayerFormat = ".html";

    private static readonly string ImagesLayerFormat = ".json";

    public static bool FilesExists(string source)
    {
        string textFile = "Files/" + source + TextLayerFormat;
        
        string imagesFile = "Files/" + source + ImagesLayerFormat;

        if (!File.Exists(textFile))
            return false;
        
        if (!File.Exists(imagesFile))
            return false;
        
        return true;
    }

    /// <summary>
    /// read text from source
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static async Task<string?> GetNoteText(string source)
    {
        string path = "Files/" + source + TextLayerFormat;

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
        string source = $"Files/{fileName}" + TextLayerFormat;

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
        string fileName = $"{Guid.NewGuid()}" + TextLayerFormat;

        string source = $"Files/{fileName}";

        await using StreamWriter sw = new StreamWriter(source);

        await sw.WriteLineAsync(text);

        return fileName;
    }

    /// <summary>
    /// Create note images layer
    /// </summary>
    /// <returns>source</returns>
    public static async Task<string> CreateNoteFiles()
    {
        Guid id = Guid.NewGuid();

        string fileNameText = $"{id}" + TextLayerFormat;

        string sourceText = $"Files/{fileNameText}";

        await using (var stream = File.Create(sourceText))
            stream.Close();

        return id.ToString();
    }
}