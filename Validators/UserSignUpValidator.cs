using ExpenseManagementMVC.DTO;
using FluentValidation;

namespace ExpenseManagementMVC.Validators
{
    public class UserSignUpValidator : AbstractValidator<UserSignUpDTO>
    {
        public UserSignUpValidator() 
        {
            RuleFor(u => u.Password)
                .Equal(u => u.ConfirmPassword)
                .WithMessage("Password is not the same in both fields.");
        }
    }
}
