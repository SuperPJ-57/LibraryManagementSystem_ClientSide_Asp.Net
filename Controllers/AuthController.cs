using ClientSideLibraryManagementSystem.Models;
using ClientSideLibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClientSideLibraryManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService,IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLoginModel)
        {
            if (ModelState.IsValid)
            {
                // Authenticate and retrieve the token
                string token = await _authService.AuthenticateAsync(userLoginModel);

                if (!string.IsNullOrEmpty(token))
                {
                    // Store the token in session
                    HttpContext.Session.SetString("JWToken", token);
                    HttpContext.Session.SetString("Username", userLoginModel.Username);
                    // Retrieve user details after authentication
                    //var userDetails = await _userService.GetUserByUsernameAsync(userLoginModel.Username);

                    //if (userDetails != null)
                    //{
                    //    Store user details in session
                    //    HttpContext.Session.SetString("UserDetails", JsonSerializer.Serialize(userDetails));
                    //    var userDetailsJson = HttpContext.Session.GetString("UserDetails");
                    //    Redirect to the dashboard on successful login
                    //    return RedirectToAction("Index", "Dashboard",userLoginModel.Username);
                    //}
                    // Redirect to the dashboard on successful login
                    return RedirectToAction("Index","Dashboard");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(userLoginModel);
        }

        //logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
