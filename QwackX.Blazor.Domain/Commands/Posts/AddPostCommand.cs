using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class AddPostCommand : ICommandDefinition
{
        public int UserId { get; }
        public string Title { get; }
        public string? Description { get; }

        public AddPostCommand(int userId, string title, string? description)
        {
                UserId = userId;
                Title = title;
                Description = description;
        }
}