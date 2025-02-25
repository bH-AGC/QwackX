using System.Text.Json.Serialization;

namespace QwackX.Blazor.Domain.Entities
{
    public class PostTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Author { get; set; }
        
        public int UserId { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }

        [JsonConstructor]
        internal PostTitle(int id, string title, DateTime createdAt, string author, int userId, int likeCount, int replyCount)
        {
            Id = id;
            Title = title;
            CreatedAt = createdAt;
            Author = author;
            UserId = userId;
            LikeCount = likeCount;
            ReplyCount = replyCount;
        }
    }
}