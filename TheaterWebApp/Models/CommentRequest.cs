using System.ComponentModel.DataAnnotations;

namespace TheaterWebApp.Models;

public record CommentRequest
{
    // 새 댓글 생성 시에는 필요 없음. 수정 시에만 사용
    public long? CommentId { get; set; }
    [Length(minimumLength:2, maximumLength:5000)]
    [Required(ErrorMessage = "내용은 필수입니다.")]
    public string Comment { get; set; } = string.Empty;

    // 부모 댓글 ID가 없으면 일반 댓글, 있으면 대댓글
    public long? ParentCommentId { get; set; }
}