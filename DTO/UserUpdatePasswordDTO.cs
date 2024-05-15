using System.ComponentModel.DataAnnotations;

namespace ExpenseManagementMVC.DTO
{
    public class UserUpdatePasswordDTO : BaseDTO
    {
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d).{5,}$",
            ErrorMessage = "Password must be at least 5 characters and should contain at least " +
                "one uppercase letter, one lowercase letter and one digit.")]
        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }
    }
}
