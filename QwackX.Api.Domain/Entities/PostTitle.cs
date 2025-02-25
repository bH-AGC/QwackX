namespace QwackX.Api.Domain.Entities;

public class PostTitle
{
    public int PostId { get; set; }
    public required string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Author { get; set; }
    public int UserId {get; set;}
    public int LikeCount { get; set; }
    public int ReplyCount { get; set; }
}