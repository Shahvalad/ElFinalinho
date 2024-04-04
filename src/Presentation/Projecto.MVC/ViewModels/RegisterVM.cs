using System.ComponentModel.DataAnnotations;

namespace Projecto.MVC.ViewModels
{
    public class RegisterVM
    {
        public string Username { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage ="Passwords do not match!")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
