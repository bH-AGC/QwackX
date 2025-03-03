namespace QwackX.Api.Domain.Entities;

public class PostView
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public DateTime ViewedAt {get; set;}
}