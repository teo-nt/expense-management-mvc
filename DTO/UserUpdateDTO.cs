using System.ComponentModel.DataAnnotations;

namespace ExpenseManagementMVC.DTO
{
    public class UserUpdateDTO : BaseDTO
    {
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Lastname should be 2-50 characters.")]
        public required string Lastname { get; set; }
    }
}
