namespace TheaterWebApp.Models;

public record MovieImageModel
{
    public string ImageName { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}