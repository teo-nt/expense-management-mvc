using AutoMapper;
using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Models;
using ExpenseManagementMVC.Repository;
using ExpenseManagementMVC.Services.Exceptions;

namespace ExpenseManagementMVC.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExpenseService> _logger;
        private readonly IMapper _mapper;

        public ExpenseService(IUnitOfWork unitOfWork, ILogger<ExpenseService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> AddExpenseAsync(ExpenseInsertDTO dto, string username)
        {
            User? user;
            Expense expense = _mapper.Map<Expense>(dto);

            try
            {
                user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
                if (user == null)
                {
                    throw new UserNotFoundException("User with username: " + username + " was not found");
                }
                await _unitOfWork.ExpenseRepository.AddAsync(expense);
                expense.User = user;
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Expense was added for user with username: " + username);
            }
            catch (Exception e)
            {
                _logger.LogError("Error inserting expense for user with username: " + username + " -- " + e.Message);
                throw;
            }
            return true;
        }

        public async Task<bool> DeleteExpenseAsync(int id, string username)
        {
            User? user;
            Expense? expense;

            try
            {
                user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
                if (user == null)
                {
                    throw new UserNotFoundException("User with username: " + username + " was not found");
                }
                expense = await _unitOfWork.ExpenseRepository.GetAsync(id);
                if (expense == null)
                {
                    throw new ExpenseNotFoundException("Expense with id: " + id + " was not found");
                }
                if (expense.User.Id != user.Id)
                {
                    throw new NotAllowedActionException("Expense does not belong to this user");
                }
                await _unitOfWork.ExpenseRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Expense with id: " + id + " was deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError("Error deleting expense with id: " + id + " -- " + e.Message);
                throw;
            }
            return true;
        }

        public async Task<List<Expense>> GetAllExpensesByUsernameAsync(string username)
        {
            User? user;
            List<Expense> expenses;

            try
            {
                user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
                if (user is null )
                {
                    throw new UserNotFoundException("User with username: " + username + " was not found");
                }
                expenses = user.Expenses.ToList();
                _logger.LogInformation("All expenses for user: " + username + " were retrieved");
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error getting all expenses for user: " + username + " -- " + e.Message);
                throw;
            }
            return expenses;
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            Expense? expense;
            try
            {
                expense = await _unitOfWork.ExpenseRepository.GetAsync(id);
                if (expense is null)
                {
                    throw new ExpenseNotFoundException("Expense with id: " + id + " was not found");
                }
                _logger.LogInformation($"Expense with id: {id} was retrieved");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting expense with id: {id} -- {e.Message}");
                throw;
            }
            return expense;
        }

        public async Task<bool> UpdateExpenseAsync(ExpenseUpdateDTO dto, int userId)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.GetAsync(userId);
                if (user is null)
                {
                    throw new UserNotFoundException("User with id: " + userId + " was not found");
                }
                Expense updatedExpense = _mapper.Map<Expense>(dto);
                Expense? existingExpense = await _unitOfWork.ExpenseRepository.GetAsync(updatedExpense.Id);
                if (existingExpense is null)
                {
                    throw new ExpenseNotFoundException("Expense with id: " + dto.Id + " was not found");
                }
                if (existingExpense.UserId != userId)
                {
                    throw new NotAllowedActionException("Expense does not belong to this user");
                }
                existingExpense.Name = updatedExpense.Name;
                existingExpense.Amount = updatedExpense.Amount;
                existingExpense.Category = updatedExpense.Category;
                existingExpense.Date = updatedExpense.Date;
                await _unitOfWork.SaveAsync();
                _logger.LogInformation($"User with id: {userId} sucessfully updated expense with id: {dto.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error updating expense with id: {dto.Id} -- {e.Message}");
                throw;
            }
            return true;
        }
    }
}
