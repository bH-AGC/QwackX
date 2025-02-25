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
        [StringLength(3000, MinimumLength = 1, ErrorMessage = "Le contenu de la description doit comporter entre 1 et 3000 caractères.")]
        public string Description { get; set; } = default!;
    }
}