using System.ComponentModel.DataAnnotations;

namespace TheaterWebApp.Entities;

public class Movie : BaseEntity
{
    public long Id { get; set; }

    [Required(ErrorMessage = "제목은 필수입니다.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "설명은 필수입니다.")]
    public string Description { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    // 1: N
    public virtual ICollection<MovieImage>? MovieImages { get; set; } = new List<MovieImage>();
    public virtual ICollection<MovieGenre>? MovieGenres { get; set; } = new List<MovieGenre>();
    public virtual ICollection<MovieRating> MovieRatings { get; set; } = new List<MovieRating>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public override string ToString() => $"Movie(Title={Title}, Description={Description})";
}