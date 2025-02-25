namespace QwackX.Api.Domain.Entities
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public required string Username { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
