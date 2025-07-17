namespace TheaterWebApp.Models;

public record CommentModel
{
    public long Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string UserNickname { get; set; } = string.Empty;

    public long? ParentCommentId { get; set; }
    public ICollection<CommentModel>? NestedComment { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}