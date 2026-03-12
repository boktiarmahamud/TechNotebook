using System.ComponentModel.DataAnnotations;

namespace TechNotebook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Post> Posts { get; set; }// Navigation property for related posts

	}
}
