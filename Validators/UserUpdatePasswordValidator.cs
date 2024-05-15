using ExpenseManagementMVC.DTO;
using FluentValidation;

namespace ExpenseManagementMVC.Validators
{
    public class UserUpdatePasswordValidator : AbstractValidator<UserUpdatePasswordDTO>
    {
        public UserUpdatePasswordValidator()
        {
            RuleFor(u => u.Password)
                .Equal(u => u.ConfirmPassword)
                .WithMessage("Password is not the same in both fields.");
        }
    }
}
