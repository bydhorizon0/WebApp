using TheaterWebApp.Entities;

namespace TheaterWebApp.Models;

public record MovieDetailViewModel
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; }
    public ICollection<MovieImageModel>? MovieImages { get; set; }
    public ICollection<string>? Genres { get; set; }
    public double MovieRating { get; set; } = 0.0;
    public ICollection<CommentModel>? Comments { get; set; }
    
    public override string ToString()
    {
        var images = MovieImages != null ? string.Join(", ", MovieImages.Select(mi => mi?.ImageName)) : "None";
        var genres = Genres != null ? string.Join(", ", Genres) : "None";
        var comments = Comments != null ? string.Join(", ", Comments.Select(c => c?.Content)) : "None";

        return $"MovieDetailViewModel {{ Id = {Id}, Title = {Title}, Description = {Description}, ReleaseDate = {ReleaseDate}, " +
               $"MovieImages = [{images}], Genres = [{genres}], MovieRating = {MovieRating}, Comments = [{comments}] }}";
    }
}