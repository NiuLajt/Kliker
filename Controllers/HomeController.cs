using Kliker.Models;
using Kliker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

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

        [Authorize]
        public IActionResult Dashboard() // returns main page (like Index.cshtml) for signed in user
        {
            return View();
        }
    }
}