using System.ComponentModel.DataAnnotations;

namespace TheaterWebApp.Entities;

public class Comment : BaseEntity
{
    public long Id { get; set; }
    [Required]
    public string Content { get; set; }

    public long UserId { get; set; }
    public virtual User User { get; set; }

    public long MovieId { get; set; }
    public virtual Movie Movie { get; set; }

    // 대댓글 Self-Referencing
    public long? ParentCommendId { get; set; } 
    public virtual Comment ParentComment { get; set; } // 부모 댓글
    public virtual ICollection<Comment> NestedComments { get; set; } = new List<Comment>(); // 자식 댓글들, 부모 댓글이 있을 시 조회 가능

    public override string ToString() => $"Comment(Content={Content})";
}