using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechNotebook.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="email must be rigt format")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password" ,ErrorMessage ="Password not match") ]
		[DataType(DataType.Password)]
		public string ConfirmPasseord { get; set; }
    }
}
