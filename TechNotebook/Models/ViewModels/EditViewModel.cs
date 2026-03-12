using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechNotebook.Models.ViewModels
{
    public class EditViewModel
    {
		public Post Post { get; set; }
		[ValidateNever]
		public List<SelectListItem> Categories { get; set; }
		[ValidateNever]
		public IFormFile FeatureImage { get; set; }   // must be here
	}
}
