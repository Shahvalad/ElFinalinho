using System.ComponentModel.DataAnnotations;

namespace Projecto.MVC.ViewModels
{
    public class LoginVM
    {
        public string Username { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
