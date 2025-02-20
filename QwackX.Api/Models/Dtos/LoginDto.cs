using System.ComponentModel.DataAnnotations;

namespace QwackX.Api.Models.Dtos;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    
    [Required]
    [StringLength(255, MinimumLength = 6)]
    public string Password { get; set; } = default!;
}