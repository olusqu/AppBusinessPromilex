using System.ComponentModel.DataAnnotations;

namespace WebBusinessPromilexApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Wprowadź nazwę użytkownika")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wprowadź hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}