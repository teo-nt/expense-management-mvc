using ExpenseManagementMVC.Models;
using ExpenseManagementMVC.Security;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagementMVC.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ExpensesDbContext context) : base(context)
        {

        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context!.Users.Where(u => u.Username == username).Include(u => u.Expenses).FirstOrDefaultAsync();
        }

        public async Task<User?> LoginUserAsync(string username, string password)
        {
            var user = await _context!.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            if (!EncryptionUtil.IsValidPasswd(password, user.Password))
            {
                return null;
            }
            return user;
        }

        public void UpdatePassword(User user)
        {
            user.Password = EncryptionUtil.EncryptPassword(user.Password);
            _context!.Users.Update(user);
        }
    }
}
