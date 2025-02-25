using System.ComponentModel.DataAnnotations;

namespace QwackX.Blazor.Models.Forms
{
    public class AddPostForm
    {
        [Required] 
        public string Title { get; set; } = default!;

        public string? Description { get; set; }
    }
}