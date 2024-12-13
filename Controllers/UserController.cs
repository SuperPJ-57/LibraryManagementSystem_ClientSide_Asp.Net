using ClientSideLibraryManagementSystem.Models;
using ClientSideLibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSideLibraryManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        public UserController(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(AddUserModel model)
        {

            if (ModelState.IsValid == false)
            {
                return RedirectToAction("Index");
            }
            var user = new UsersEntity
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };
            var result = await _userService.AddUserAsync(user);
            if (!result)
            {
                ModelState.AddModelError("", "Error adding user.");
                return RedirectToAction("Index");
                
            }
            return RedirectToAction("Login","Auth");
        }
    }
}
