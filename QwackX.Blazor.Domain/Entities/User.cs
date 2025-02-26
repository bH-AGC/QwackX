using System.Text.Json.Serialization;

namespace QwackX.Blazor.Domain.Entities;

public class User
{
    public int UserId { get; }
    public string Username { get; }
    public string Email { get; }
    public string Password { get; }
    public DateTime CreatedAt { get; }

    [JsonConstructor]
    internal User(int userId, string username, string email, string password, DateTime createdAt)
    {
        UserId = userId;
        Username = username;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
    }
}