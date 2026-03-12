using System.ComponentModel.DataAnnotations;

namespace TechNotebook.Models.ViewModels
{
    public class LoginViewModel
    {
		[Required]
		[EmailAddress(ErrorMessage = "email must be rigt format")]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
	}
}
