using ExpenseManagementMVC.DTO;
using FluentValidation;

namespace ExpensesManagementMVC.Validators
{
    public class ExpenseInsertValidator : AbstractValidator<ExpenseInsertDTO>
    {
        public ExpenseInsertValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .WithMessage("'Name' should not be empty")
                .Length(3, 40)
                .WithMessage("'Name' should be 3-40 characters");

            RuleFor(e => e.Amount)
                .NotEmpty()
                .WithMessage("'Amount' should be a number")
                .GreaterThan(0)
                .WithMessage("'Amount' should be positive")
                .PrecisionScale(12, 2, true)
                .WithMessage("'Amount' must not be more than 12 digits in total, with allowance for 2 decimals.");

            RuleFor(e => e.Category)
                .NotEmpty()
                .WithMessage("'Category' should be selected")
                .Length(3, 20)
                .WithMessage("'Category' should be 3-20 characters");
        }
    }
}
