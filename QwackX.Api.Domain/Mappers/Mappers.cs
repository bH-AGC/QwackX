using System.Data;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Mappers;

internal static partial class Mappers
{
    public static User ToUser(this IDataRecord record)
    {
        return new User
        {
            UserId = (int)record["Id"],
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
            PostId = (int)record["Id"],
            UserId = (int)record["UserId"],
            Title = (string)record["Title"],
            CreatedAt = (DateTime)record["CreatedAt"],
            Author = (string)record["Username"],
            LikeCount = (int)record["LikeCount"],
            ReplyCount = (int)record["ReplyCount"],
            ViewCount = (int)record["ViewCount"],
            IsLiked = (int)record["IsLiked"] == 1
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
            Author = (string)record["Username"],
            UserId = (int)record["UserId"],
            LikeCount = (int)record["LikeCount"],
            IsLiked = (int)record["IsLiked"] == 1
        };
    }
    
    public static Reply ToReply(this IDataRecord record)
    {
        return new Reply
        {
            ReplyId = (int)record["Id"],
            Content = (string)record["Content"],
            CreatedAt = (DateTime)record["CreatedAt"],
            Author = (string)record["Username"],
            LikeCount = (int)record["LikeCount"],
            IsLiked = (int)record["IsLiked"] == 1
        };
    }
}
