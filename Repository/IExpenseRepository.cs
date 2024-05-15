using ExpenseManagementMVC.Models;

namespace ExpenseManagementMVC.Repository
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetByUsernameAsync(string username);
    }
}
