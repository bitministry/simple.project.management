using Glen.Domain.Entities;

namespace Glen.MVC.Models
{


    public class LoginViewModel : Login
    {
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string Email { get; set; }

        public bool PasswordReminder { get; set; }
        
    }
}