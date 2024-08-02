using Microsoft.AspNetCore.Mvc;
using PCM.Services;

namespace PCM.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> ManageUsers()
        {

            if (await IsAdmin())
            {
                var users = await _userService.GetAllUsersAsync();
                return View(users);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> BlockUsers([FromBody] List<Guid> userIds)
        {
            await _userService.BlockUsersAsync(userIds);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsers([FromBody] List<Guid> userIds)
        {
            await _userService.UnblockUsersAsync(userIds);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers([FromBody] List<Guid> userIds)
        {
            await _userService.DeleteUsersAsync(userIds);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MakeAdmins([FromBody] List<Guid> userIds)
        {
            await _userService.MakeAdminsAsync(userIds);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdmins([FromBody] List<Guid> userIds)
        {
            await _userService.RemoveAdminsAsync(userIds);
            return Ok();
        }


        public async Task<bool> IsAdmin()
        {
            var id = Guid.Parse(HttpContext.Session.GetString("Id"));

            return await _userService.IsAdminAsync(id);


        }
    }
}
