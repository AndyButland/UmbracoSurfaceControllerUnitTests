namespace SurfaceControllerUnitTests.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Enter a comment")]
        public string Comment { get; set; }
    }
}