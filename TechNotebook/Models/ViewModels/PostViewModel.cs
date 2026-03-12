using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechNotebook.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TechNotebook.Models.ViewModels
{
	public class PostViewModel
	{
		public Post Post { get; set; }
		[ValidateNever]
		public List<SelectListItem> Categories { get; set; }
		public IFormFile FeatureImage { get; set; }   // must be here
	}
}
