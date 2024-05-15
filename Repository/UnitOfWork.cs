
using ExpenseManagementMVC.Models;

namespace ExpenseManagementMVC.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExpensesDbContext _context;

        public UnitOfWork(ExpensesDbContext context)
        {
            _context = context;
        }

        public UserRepository UserRepository => new UserRepository(_context);
        public ExpenseRepository ExpenseRepository => new ExpenseRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
