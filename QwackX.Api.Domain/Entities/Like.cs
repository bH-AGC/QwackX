namespace QwackX.Api.Domain.Entities;

public class Like
{
    public int LikeId { get; set; }
    public int UserId { get; set; }
    public int EntityId { get; set; }
    public DateTime CreatedAt { get; set; }
}