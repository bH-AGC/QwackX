using System.ComponentModel.DataAnnotations;

namespace QwackX.Blazor.Models.Forms;

public class LoginUserForm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}