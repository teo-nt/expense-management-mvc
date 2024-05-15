using AutoMapper;
using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Models;
using ExpenseManagementMVC.Repository;
using ExpenseManagementMVC.Services.Exceptions;

namespace ExpenseManagementMVC.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(ILogger<UserService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.GetAsync(id);
                if (user == null)
                {
                    throw new UserNotFoundException($"User with id: {id} was not found");
                }
                await _unitOfWork.UserRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation($"User with id: {id} was successfully deleted");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error deleting user with id: {id} -- {e.Message}");
                throw;
            }
            return true;
        }

        public async Task<User?> LoginUserAsync(UserLoginDTO loginDTO)
        {
            User? user;
            try
            {
                user = await _unitOfWork.UserRepository.LoginUserAsync(loginDTO.Username, loginDTO.Password);
                if (user == null)
                {
                    throw new UserNotFoundException
                        ($"This user with these credentials does not exist");
                }
                _logger.LogInformation($"User: {user.Username} successfully logged in");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in login -- {e.Message}");
                throw;
            }
            return user;
        }

        public async Task SignUpUserAsync(UserSignUpDTO signUpDTO)
        {
            try
            {
                User user = _mapper.Map<User>(signUpDTO);
                if (await _unitOfWork.UserRepository.GetByUsernameAsync(user.Username) != null)
                {
                    throw new UserAlreadyExistsException($"User: {user.Username} already exists");
                }
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation($"User: {user.Username} successfully signed in");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in signing up user -- {e.Message}");
                throw;
            }
        }

        public async Task<User?> UpdateUserDetailsAsync(UserUpdateDTO updateDTO)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.GetAsync(updateDTO.Id);
                if (user == null)
                {
                    throw new UserNotFoundException($"User with id: {updateDTO.Id} was not found");
                }
                user.Firstname = updateDTO.Firstname;
                user.Lastname = updateDTO.Lastname;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation($"User details for user with id: {user.Id} were successfully updated");
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error updating user details -- {e.Message}");
                throw;
            }
        }

        public async Task<User?> UpdateUserPasswordAsync(UserUpdatePasswordDTO updatePasswordDTO)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.GetAsync(updatePasswordDTO.Id);
                if (user is null)
                {
                    throw new UserNotFoundException($"User with id: {updatePasswordDTO.Id} was not found");
                }
                user.Password = updatePasswordDTO.Password;
                _unitOfWork.UserRepository.UpdatePassword(user);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation($"User {user.Username} successfully changed password");
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error changing password for user with id: {updatePasswordDTO.Id} -- {e.Message}");
                throw;
            }
        }
    }
}
