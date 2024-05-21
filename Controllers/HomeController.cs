using ExpenseManagementMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Services;
using AutoMapper;

namespace ExpenseManagementMVC.Controllers
{
    public class HomeController : Controller
    {
        public List<ExpenseReadOnlyDTO> ExpensesDTO { get; set; } = new();
        private readonly IApplicationService _applicationsService;
        private readonly IMapper _mapper;

        public HomeController(IApplicationService appliactionService, IMapper mapper)
        {
            _applicationsService = appliactionService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                string username = HttpContext.User.Identity!.Name!;
                List<Expense> expenses = await _applicationsService.ExpenseService.GetAllExpensesByUsernameAsync(username);
                expenses.ForEach(expense => ExpensesDTO.Add(_mapper.Map<ExpenseReadOnlyDTO>(expense)));
            } catch (Exception e)
            {
                ViewData["ErrorMessage"] = e.Message;
            }
            return View(ExpensesDTO);
        }

        [Authorize]
        public IActionResult Account()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
}
