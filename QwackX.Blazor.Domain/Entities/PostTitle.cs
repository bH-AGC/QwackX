using System.Text.Json.Serialization;

namespace QwackX.Blazor.Domain.Entities
{
    public class PostTitle
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Author { get; set; }
        public int UserId { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }

        [JsonConstructor]
        internal PostTitle(int postId, string title, DateTime createdAt, string author, int userId, int likeCount, int replyCount)
        {
            PostId = postId;
            Title = title;
            CreatedAt = createdAt;
            Author = author;
            UserId = userId;
            LikeCount = likeCount;
            ReplyCount = replyCount;
        }
    }
}