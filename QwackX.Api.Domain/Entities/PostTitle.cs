namespace QwackX.Api.Domain.Entities;

public class PostTitle
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; }
    public int UserId {get; set;}
    public int LikeCount { get; set; }
    public int ReplyCount { get; set; }
}