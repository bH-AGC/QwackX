using System.Data;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Mappers;

internal static partial class Mappers
{
    public static User ToUser(this IDataRecord record)
    {
        return new User
        {
            Id = (int)record["Id"],
            Username = (string)record["Username"],
            Email = (string)record["Email"],
            Password = (string)record["PasswordHash"],
            CreatedAt = (DateTime)record["CreatedAt"]
        };
    }

    public static PostTitle ToPostTitle(this IDataRecord record)
    {
        return new PostTitle
        {
            Id = (int)record["Id"],
            UserId = (int)record["UserId"],
            Title = (string)record["Title"],
            CreatedAt = (DateTime)record["CreatedAt"],
            Username = (string)record["Username"],
            LikeCount = (int)record["LikeCount"],
            ReplyCount = (int)record["ReplyCount"]
        };
    }
    
    public static Post ToPost(this IDataRecord record)
    {
        return new Post
        {
            PostId = (int)record["Id"],
            Title = (string)record["Title"],
            Description = (string)record["Description"],
            CreatedAt = (DateTime)record["CreatedAt"],
            Username = (string)record["Username"],
            UserId = (int)record["UserId"],
            LikeCount = (int)record["LikeCount"],
        };
    }
    
    // public static Reply ToReply(this IDataRecord record)
    // {
    //     return new Reply
    //     {
    //         ReplyId = (int)record["ReplyId"],
    //         ReplyContent = (string)record["ReplyContent"],
    //         ReplyCreatedAt = (DateTime)record["ReplyCreatedAt"],
    //         ReplyUsername = (string)record["ReplyUsername"],
    //         ReplyUserId = (int)record["UserId"]
    //     };
    // }
}
