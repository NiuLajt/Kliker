using Kliker.Models;
using Kliker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kliker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, errorType = "INVALID_FORM" });

            // check if user already exists in database
            if (_userService.IsUsernameAvailable(model.Username)) return Json(new { success = false, errorType = "LOGIN_TAKEN" });

            if (_userService.IsMailAvailable(model.Mail)) return Json(new { success = false, errorType = "MAIL_TAKEN" });

            _userService.AddUserToDatabase(model);

            return Json(new {success = true});
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model) 
        {
            if (!ModelState.IsValid) return Json(new { success = false, errorType = "INVALID_FORM" });

            // check if user exists (mail or username in database)
            if (!_userService.IsUserAvailableByUsernameOrMail(model.Username)) return Json(new { success = false, errorType = "USER_NOT_FOUND" });

            if (!_userService.ValidateUserByPassword(model.Username, model.Password)) return Json(new { success = false, errorType = "INVALID_CREDENTIALS" });

            var user = _userService.GetUserFromDatabase(model.Username);
            if(user is null) return Json(new { success = false, errorType = "USER_NOT_FOUND" });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Home") });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        { 
            _logger.LogInformation("Próba wylogowania...");
            await HttpContext.SignOutAsync("Cookies");
            return Json(new { success = true });
        }


        [Authorize]
        public IActionResult UserData()
        {
            // check cookies for authentication and send data about user to browser
            var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            if (usernameClaim == null) return Json(new { success = false, errorType = "USER_NOT_FOUND" });

            var username = usernameClaim.Value;
            var user = _userService.GetUserFromDatabase(username);
            if (user == null) return Json(new { success = false, errorType = "USER_NOT_FOUND" });

            return Json(new
            {
                success = true,
                userId = user.Id,
                username = user.Username,
                points = user.Points,
                level = user.Lvl
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdatePoints([FromBody] UpdatePointsModel model)
        {
            if (model is null || string.IsNullOrEmpty(model.Username)) return Json(new { success = false, errorType = "INVALID_DATA" });

            var user = _userService.GetUserFromDatabase(model.Username);
            if (user == null) return Json(new { success = false, errorType = "USER_NOT_FOUND" });

            _userService.UpdatePoints(model);

            return Json(new { success = true });
        }
    }
}