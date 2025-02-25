using System.Text.Json.Serialization;

namespace QwackX.Blazor.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Author { get; set; }
        public int UserId { get; set; }
        public int LikeCount { get; set; }

        [JsonConstructor]
        internal Post(int id, string title, string description, DateTime createdAt, string author, int userId,
            int likeCount)
        {
            Id = id;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            Author = author;
            UserId = userId;
            LikeCount = likeCount;
        }
    }
}