using System.ComponentModel.DataAnnotations;

namespace StellumbraSite.Model
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a valid Username.")]
        public string? Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a valid Password.")]
        public string? Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
