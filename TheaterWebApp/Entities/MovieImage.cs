namespace TheaterWebApp.Entities;

public class MovieImage : BaseEntity
{
    public long Id { get; set; }
    public string ImageName { get; set; }
    public string Path { get; set; }

    public long MovieId { get; set; }
    public virtual Movie Movie { get; set; }
}