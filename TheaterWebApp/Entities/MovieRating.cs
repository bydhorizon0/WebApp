

namespace TheaterWebApp.Entities;

public class MovieRating : BaseEntity
{
    public long Id { get; set; }
    public int Score { get; set; }
    
    public long MovieId { get; set; }
    public virtual Movie Movie { get; set; }

    public long UserId { get; set; }
    public virtual User User { get; set; }
}