using System.ComponentModel.DataAnnotations;

namespace ExpenseManagementMVC.DTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}
