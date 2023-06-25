namespace Domain.Note.Layers;

public class ImageNoteDomain
{
    public Guid Id { get; set; }
    
    public int X { get; set; }
    
    public int Y { get; set; }
    
    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public string? SvgCode { get; set; }

    public string? Url => $"https://localhost:7058/{Id}.png";
}