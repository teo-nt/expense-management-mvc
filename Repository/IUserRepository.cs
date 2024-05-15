using ExpenseManagementMVC.Models;

namespace ExpenseManagementMVC.Repository
{
    public interface IUserRepository
    {
        Task<User?> LoginUserAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
        void UpdatePassword(User user);
    }
}
