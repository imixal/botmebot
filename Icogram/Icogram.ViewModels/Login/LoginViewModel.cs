using System.ComponentModel.DataAnnotations;

namespace Icogram.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}