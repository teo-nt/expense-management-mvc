using ExpenseManagementMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagementMVC.Repository
{
    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ExpensesDbContext context) : base(context) { }

        public async Task<IEnumerable<Expense>> GetByUsernameAsync(string username)
        {
            return await _context!.Expenses.Where(e => e.User.Username == username).ToListAsync();
        }
    }
}
