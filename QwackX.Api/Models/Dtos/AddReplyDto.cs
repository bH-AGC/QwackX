using System.ComponentModel.DataAnnotations;

namespace QwackX.Api.Models.Dtos
{
    public class AddReplyDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Le contenu de la réponse doit comporter entre 1 et 1000 caractères.")]
        public string Content { get; set; } = default!;
    }
}