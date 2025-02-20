using System.ComponentModel.DataAnnotations;

namespace QwackX.Api.Models.Dtos
{
    public class AddUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; } = default!;
    }
}