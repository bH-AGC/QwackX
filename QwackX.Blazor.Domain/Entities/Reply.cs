using System.Text.Json.Serialization;

namespace QwackX.Blazor.Domain.Entities;

public class Reply
{
    public int ReplyId { get; set; }
    public required string Author { get; set; }
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    [JsonConstructor]
    public Reply(int replyId, string author, string content, DateTime createdAt)
    {
        ReplyId = replyId;
        Author = author;
        Content = content;
        CreatedAt = createdAt;
    }
}