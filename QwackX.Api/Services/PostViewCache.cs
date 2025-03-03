using System.Collections.Concurrent;

namespace QwackX.Api.Services
{
    public class PostViewCache
    {
        private readonly ConcurrentDictionary<int, HashSet<(int userId, DateTime viewedAt)>> _views = new();
        
        public void AddView(int postId, int userId, DateTime viewedAt)
        {
            _views.AddOrUpdate(postId,
                _ => new HashSet<(int userId, DateTime viewedAt)> { (userId, viewedAt) },
                (_, users) =>
                {
                    users.Add((userId, viewedAt));
                    return users;
                });
        }
        
        public Dictionary<int, HashSet<(int userId, DateTime viewedAt)>> GetViewsAndClear()
        {
            var snapshot = new Dictionary<int, HashSet<(int userId, DateTime viewedAt)>>(_views);
            _views.Clear();
            return snapshot;
        }
    }
}