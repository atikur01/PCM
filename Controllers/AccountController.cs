using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PCM.Models;
using PCM.Services;

namespace PCM.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string Name)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.RegisterUserAsync(email, password, Name);
                if (user == null)
                {
                    ModelState.AddModelError("", "This email is already registered.");
                    return View();
                }
                //After successfull registration, redirect to login page
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string Email, string password)
        {
            var user = await _userService.AuthenticateUserAsync(Email, password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }

            var sessionData = new Dictionary<string, string>
            {
                { "Id", user.UserId.ToString() },
                { "Name", user.Name },
                { "Email", user.Email },
                { "Role", user.Role },
                { "IsBlocked", user.IsBlocked.ToString() }
            };

            AddSessionData(sessionData);

            if (user.Role == UserRole.Admin)
            {
                return RedirectToAction("ManageUsers", "Admin");
            }
            if (user.Role == UserRole.User)
            {
                return RedirectToAction("IndexByUserID", "Collection", new { userid = user.UserId });
            }

            return View();
        }


        public void AddSessionData(Dictionary<string, string> sessionData)
        {
            foreach (var data in sessionData)
            {
                HttpContext.Session.SetString(data.Key, data.Value);
            }
        }


        public async Task<IActionResult> Logout()
        {
            string sessionUserIdString = HttpContext.Session.GetString("Id") ?? string.Empty;

            if (string.IsNullOrEmpty(sessionUserIdString))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var GuidId = Guid.Parse(sessionUserIdString);

            await _userService.LogoutAsync(GuidId);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");

        }
    }
}