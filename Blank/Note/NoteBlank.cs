namespace Blank.Note;

public class NoteBlank
{
    public string Header { get; set; } = null!;

    public string? Text { get; set; }

    public List<int> Tags { get; set; } = new List<int>();
}