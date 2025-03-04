using System.ComponentModel.DataAnnotations;

namespace QwackX.Api.Models.Dtos
{
    public class ViewPostDto
    {
        [Required]
        public int PostId { get; set; }
        
        [Required]
        public int UserId { get; set; }
    }
}