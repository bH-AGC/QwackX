using System.Text.Json.Serialization;

namespace QwackX.Blazor.Domain.Entities;

public class User
{
    public int Id { get; }
    public string Username { get; }
    public string Email { get; }
    public string PasswordHash { get; }
    public DateTime CreatedAt { get; }

    [JsonConstructor]
    internal User(int id, string username, string email, string passwordHash, DateTime createdAt)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
    }
}