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
                ModelState.Clear();
                return View(model);
            }

            // check if user already exists in database
            if (!_userService.IsUsernameAvailable(model.Username))
            {
                ModelState.AddModelError(string.Empty, "Login niedostepny. Wybierz inny login.");
                return View(model);
            }
            if (!_userService.IsMailAvailable(model.Mail))
            {
                ModelState.AddModelError(string.Empty, "Adres e-mail niedostepny. Użytkownik z tym adresem istnieje już w bazie.");
                return View(model);
            }

            _userService.AddUserToDatabase(model);

            return View();
        }
    }
}