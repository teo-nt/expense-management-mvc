using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Models;

namespace ExpenseManagementMVC.Services
{
    public interface IExpenseService
    {
        Task<bool> AddExpenseAsync(ExpenseInsertDTO dto, string username);
        Task<bool> UpdateExpenseAsync(ExpenseUpdateDTO dto, int userId);
        Task<bool> DeleteExpenseAsync(int id, string username);
        Task<List<Expense>> GetAllExpensesByUsernameAsync(string username);
        Task<Expense> GetExpenseByIdAsync(int id);
    }
}
