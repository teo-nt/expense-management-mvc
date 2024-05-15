using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Models;

namespace ExpenseManagementMVC.Services
{
    public interface IUserService
    {
        Task SignUpUserAsync(UserSignUpDTO signUpDTO);
        Task<User?> LoginUserAsync(UserLoginDTO loginDTO);
        Task<User?> UpdateUserDetailsAsync(UserUpdateDTO updateDTO);
        Task<User?> UpdateUserPasswordAsync(UserUpdatePasswordDTO updatePasswordDTO);
        Task<bool> DeleteUserAsync(int id);
    }
}
