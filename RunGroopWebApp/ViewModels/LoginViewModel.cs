using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}