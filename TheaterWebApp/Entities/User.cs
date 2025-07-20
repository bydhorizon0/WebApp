using System.ComponentModel.DataAnnotations;

namespace TheaterWebApp.Entities;

public class User : BaseEntity
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<MovieRating> Ratings { get; set; } = new  List<MovieRating>();
}