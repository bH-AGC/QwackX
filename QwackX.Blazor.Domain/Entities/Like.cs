using System.Text.Json.Serialization;

namespace QwackX.Blazor.Domain.Entities;

public class Like
{
    public int LikeId { get; set; }
    public int UserId { get; set; }
    public int EntityId { get; set; }
    public string EntityType { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    [JsonConstructor]
    public Like(int likeId, int userId, int entityId, string entityType, DateTime createdAt)
    {
        LikeId = likeId;
        UserId = userId;
        EntityId = entityId;
        EntityType = entityType;
        CreatedAt = createdAt;
    }
}