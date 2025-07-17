using System.ComponentModel.DataAnnotations;

namespace TheaterWebApp.Entities;

public class User : BaseEntity
{
    public long Id { get; set; }

    [EmailAddress] 
    [Required] 
    public string Email { get; set; } = string.Empty;
    
    [Required] 
    public string Nickname { get; set; } = string.Empty;
    
    [Required] 
    public string Password { get; set; } = string.Empty;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}