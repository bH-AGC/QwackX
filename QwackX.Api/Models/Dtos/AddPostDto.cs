using System.ComponentModel.DataAnnotations;

namespace QwackX.Api.Models.Dtos
{
    public class AddPostDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Title { get; set; } = default!;
        
        [Required]
        public string Description { get; set; } = default!;
    }
}