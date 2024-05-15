using System.ComponentModel.DataAnnotations;

namespace ExpenseManagementMVC.DTO
{
    public class UserSignUpDTO
    {
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Lastname should be 2-50 characters.")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username should be 4-50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d).{5,}$", 
            ErrorMessage = "Password must be at least 5 characters and should contain at least " + 
                "one uppercase letter, one lowercase letter and one digit.")]     
        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
