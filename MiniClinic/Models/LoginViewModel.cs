using System.ComponentModel.DataAnnotations;

namespace MiniClinic.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email adress!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password needs to be between 8 and 20 caratcters long!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
