using System.ComponentModel.DataAnnotations;

public class LikeDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int EntityId { get; set; }

    [Required]
    [RegularExpression("Post|Reply", ErrorMessage = "EntityType doit être 'Post' ou 'Reply'.")]
    public string EntityType { get; set; } = default!;
}