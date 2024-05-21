using AutoMapper;
using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Models;
using ExpenseManagementMVC.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseManagementMVC.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IApplicationService _applicationService;
        public ExpenseInsertDTO ExpenseInsertDTO { get; set; } = new();
        public List<Error> Errors { get; set; } = new();
        private readonly IValidator<ExpenseInsertDTO> _insertValidator;
        private readonly IValidator<ExpenseUpdateDTO> _updateValidator;
        private readonly IMapper _mapper;

        public ExpenseController(IApplicationService applicationService, IValidator<ExpenseInsertDTO> insertValidator,
                        IValidator<ExpenseUpdateDTO> updateValidator, IMapper mapper)
        {
            _applicationService = applicationService;
            _insertValidator = insertValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View(ExpenseInsertDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ExpenseInsertDTO expenseInsertDTO)
        {
            string username = HttpContext.User.Identity!.Name!;
            var validationResults = _insertValidator.Validate(expenseInsertDTO);
            if (!validationResults.IsValid)
            {
                foreach (var error in validationResults.Errors)
                {
                    Errors.Add(new Error(error.ErrorCode, error.ErrorMessage, error.PropertyName));
                }
                ViewData["Errors"] = Errors;
                return View(expenseInsertDTO);
            }
            await Console.Out.WriteLineAsync("Test");
            try
            {
                await _applicationService.ExpenseService.AddExpenseAsync(expenseInsertDTO, username);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                ViewData["Errors"] = e.Message;
                return View(expenseInsertDTO);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            ExpenseUpdateDTO updateDTO = new();
            try
            {
                Expense expense = await _applicationService.ExpenseService.GetExpenseByIdAsync(id);
                if (expense.UserId.ToString() != HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
                {
                    return RedirectToAction("Index", "Home");
                }
                updateDTO = _mapper.Map<ExpenseUpdateDTO>(expense);
                return View(updateDTO);
            } catch (Exception)
            {
                
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(ExpenseUpdateDTO updateDTO)
        {
            var validationResults = _updateValidator.Validate(updateDTO);
            if (!validationResults.IsValid)
            {
                validationResults.Errors.ForEach(error => 
                    Errors.Add(new Error(error.ErrorCode, error.ErrorMessage, error.PropertyName)));
                ViewData["Errors"] = Errors;
                return View(updateDTO);
            }

            _ = int.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
            try
            {
                await _applicationService.ExpenseService.UpdateExpenseAsync(updateDTO, userId);
            }
            catch (Exception)
            {
                
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
