using Kliker.Models;
using Kliker.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

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
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errorType = "INVALID_FORM" });
            }

            // check if user already exists in database
            if (_userService.IsUsernameAvailable(model.Username))
            {
                return Json(new { success = false, errorType = "LOGIN_TAKEN" });
            }
            if (_userService.IsMailAvailable(model.Mail))
            {
                return Json(new { success = false, errorType = "MAIL_TAKEN" });
            }

            _userService.AddUserToDatabase(model);

            return Json(new {success = true});
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return Json(new { success = false, erroryType = "INVALID_FORM" });
            }

            // check if user exists (mail or username in database)
            if (!_userService.IsUserAvailableByUsernameOrMail(model.Username, model.Password))
            {
                return Json(new { success = false, errorType = "USER_NOT_FOUND" });
            }

            if(_userService.ValidateUserByPassword(model.Username, model.Password)) return Json(new { success = true }); // TODO: return page for SIGNED IN user

            return Json(new { success = false, errorType = "UNKNOWN_ERROR" });
        }
    }
}