using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Models;
using ExpenseManagementMVC.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseManagementMVC.Controllers
{
    public class UserController : Controller
    {
        public List<Error> ErrorArray { get; set; } = new();
        private readonly IApplicationService _applicationsService;
        private readonly IValidator<UserSignUpDTO> _userSignUpValidator;

        public UserController(IApplicationService applicationsService, IValidator<UserSignUpDTO> userSignUpValidator)
        {
            _applicationsService = applicationsService;
            _userSignUpValidator = userSignUpValidator;
        }

        [HttpGet]
        public ActionResult<UserSignUpDTO> Signup()
        {
            return View(new UserSignUpDTO());
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ClaimsPrincipal principal = HttpContext.User;
            if (principal.Identity!.IsAuthenticated)
            {   
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<UserSignUpDTO>> SignUp(UserSignUpDTO userSignUpDTO)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState.Values)
                {
                    foreach (var error in entry.Errors)
                    {
                        ErrorArray.Add(new Error("", error.ErrorMessage, ""));
                    }
                }       
            }
            var validationResults = _userSignUpValidator.Validate(userSignUpDTO);
            if (!validationResults.IsValid)
            {
                foreach (var error in validationResults.Errors)
                {
                    ErrorArray.Add(new Error("", error.ErrorMessage, error.PropertyName));
                }
            }
            if (ErrorArray.Count > 0)
            {
                ViewData["ErrorArray"] = ErrorArray;
                return View(userSignUpDTO);
            }

            try
            {
                await _applicationsService.UserService.SignUpUserAsync(userSignUpDTO);
                return RedirectToAction("Login", "User");
            }
            catch (Exception e)
            {
                ErrorArray.Add(new Error("", e.Message, ""));
                ViewData["ErrorArray"] = ErrorArray;
                return View(userSignUpDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO credentials, string returnUrl)
        {
            try
            {
                var user = await _applicationsService.UserService.LoginUserAsync(credentials);
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, user!.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new()
                {
                    AllowRefresh = true,
                    IsPersistent = credentials.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
                if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["ValidateMessage"] = e.Message;
                return View();
            }
        }
    }
}
