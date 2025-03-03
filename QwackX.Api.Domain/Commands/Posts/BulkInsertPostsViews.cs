using CommandQuerySeparation.Commands;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Commands
{
    public class BulkInsertPostsViews : ICommandDefinition
    {
        public List<PostView> PostViews { get; }

        public BulkInsertPostsViews(List<PostView> postViews)
        {
            PostViews = postViews;
        }
    }
}