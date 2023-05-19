namespace Views.Note;

public class Note
{
    // for identity on site (https://{domain}/note/{code})
    public string Code { get; set; } = string.Empty;

    public string Header { get; set; } = string.Empty;
    
    public string Text { get; set; } = string.Empty;

    public DateTime CreationDateTime { get; set; }
    
    public List<Tag> Tags { get; set; } = new List<Tag>();
}