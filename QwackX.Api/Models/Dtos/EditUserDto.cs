using System.ComponentModel.DataAnnotations;

namespace QwackX.Api.Models.Dtos;

public class EditUserDto
{
    [Required]
    public int Id { get; set; }
        
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(255, MinimumLength = 6)]
    public string PasswordHash { get; set; } = default!;
}