using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class LogoutViewModel
    {
        public string LogoutId { get; set; }
    }
}
