using System;

namespace QwackX.Api.Domain.Entities;
public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; }
    
    public int UserId { get; set; }
    public int LikeCount { get; set; }
}


    // public class Reply
    // {
    //     public int ReplyId { get; set; }
    //     public string ReplyContent { get; set; }
    //     public DateTime ReplyCreatedAt { get; set; }
    //     public string ReplyUsername { get; set; }
    //     
    //     public int ReplyUserId { get; set; }
    // }