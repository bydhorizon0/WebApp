using TheaterWebApp.Entities;

namespace TheaterWebApp.Models;

public record MovieViewModel
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? MainGenre { get; set; }
    public MovieImageModel? MainImage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}