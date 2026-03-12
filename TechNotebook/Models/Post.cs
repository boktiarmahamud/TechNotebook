using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TechNotebook.Models
{
    public class Post
    {
        [Key]
		public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(400, ErrorMessage = "Title cannot exceed 400 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }
        [ValidateNever]
        public string FeaturedImageUrl { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
        [ForeignKey("Category")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
		
		// Navigation property for related comments. Use a collection and initialize it so EF Core can materialize it.
		public ICollection<Comments> Comments { get; set; } = new List<Comments>();
    
    



	}
}
