using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class LikeCommand : ICommandDefinition
{
    public int UserId { get; }
    public int EntityId { get; }
    public string EntityType { get; }

    public LikeCommand(int userId, int entityId, string entityType)
    {
        UserId = userId;
        EntityId = entityId;
        EntityType = entityType;
    }
}
