using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register(string email, string password , string Name)
        {

            if (ModelState.IsValid)
            {
                var user = await _userService.RegisterUserAsync(email, password , Name);
                if (user == null)
                {
                    ModelState.AddModelError("", "This email is already registered.");
                    return View();
                }
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

            AddSessionData(user.UserId, user.Name , user.Email, user.Role, user.IsBlocked.ToString());

            if (user.Role == UserRole.Admin)
            {
                return RedirectToAction("ManageUsers", "Admin");
            }
            if (user.Role == UserRole.User)
            {
                return RedirectToAction("IndexByUserID", "Collection" , new { userid = user.UserId });
            }

            return View();
        }


        public async void AddSessionData(Guid id, string name, string email, string role, string isBlocked)
        {
            HttpContext.Session.SetString("Id", id.ToString());
            HttpContext.Session.SetString("Name", name.ToString()); 
            HttpContext.Session.SetString("Email", email);
            HttpContext.Session.SetString("Role", role);
            HttpContext.Session.SetString("IsBlocked", isBlocked);
        }

        public async Task<IActionResult> Logout()
        {
            var GuidId = Guid.Parse(HttpContext.Session.GetString("Id"));
            await _userService.LogoutAsync(GuidId);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");

        }
    }
}
