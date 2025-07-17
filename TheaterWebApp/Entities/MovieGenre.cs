namespace TheaterWebApp.Entities;

public class MovieGenre : BaseEntity
{
    public long Id { get; set; }
    public virtual MovieGenres Genre { get; set; }

    public long MovieId { get; set; }
    public virtual Movie Movie { get; set; }
}