using System.Text.Json;
using Database.Note.Layers;
using Domain.Note.Layers;
using Views.Note.Layers;

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
    /// Read note images from images layer
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static async Task<List<ImageNoteDatabase>?> GetNoteImages(string source)
    {
        string path = "Files/" + source + ImagesLayerFormat;

        if (!File.Exists(path))
            return null;

        using StreamReader sr = new StreamReader(path);

        var text = await sr.ReadToEndAsync();

        return JsonSerializer.Deserialize<List<ImageNoteDatabase>>(text);
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
    /// Update note images layer
    /// </summary>
    /// <param name="source"></param>
    /// <param name="images"></param>
    public static async Task UpdateNoteImages(string source, List<ImageNoteDatabase> images)
    {
        string fullPath = $"Files/{source}" + ImagesLayerFormat;

        await using StreamWriter sw = new StreamWriter(fullPath);

        string json = JsonSerializer.Serialize(images);

        await sw.WriteLineAsync(json);
    }

    /// <summary>
    /// Create note images layer
    /// </summary>
    /// <param name="images"></param>
    /// <returns></returns>
    public static async Task<string> CreateNoteImages(List<ImageNoteDatabase> images)
    {
        string fileName = $"{Guid.NewGuid()}" + ImagesLayerFormat;

        string source = $"Files/{fileName}";

        await using StreamWriter sw = new StreamWriter(source);

        string json = JsonSerializer.Serialize(images);

        await sw.WriteLineAsync(json);

        return fileName;
    }

    /// <summary>
    /// Create note images layer
    /// </summary>
    /// <returns>source</returns>
    public static async Task<string> CreateNoteFiles()
    {
        Guid id = Guid.NewGuid();

        string fileNameImages = $"{id}" + ImagesLayerFormat;

        string fileNameText = $"{id}" + TextLayerFormat;

        string sourceText = $"Files/{fileNameText}";

        string sourceImages = $"Files/{fileNameImages}";

        await using (var stream = File.Create(sourceText))
            stream.Close();

        await using (var stream = File.Create(sourceImages))
            stream.Close();

        return id.ToString();
    }
}