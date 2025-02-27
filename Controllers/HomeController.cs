using Kliker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Kliker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
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
            var existingLogin = _appDbContext.Users.FirstOrDefault(u => u.Username == model.Username);
            if (existingLogin != null)
            {
                ModelState.AddModelError(string.Empty, "Login niedostepny. Wybierz inny login.");
                return View(model);
            }
            var existingMail = _appDbContext.Users.FirstOrDefault(u => u.Email == model.Mail);
            if (existingMail != null)
            {
                ModelState.AddModelError(string.Empty, "Adres e-mail niedostepny. Użytkownik z tym adresem istnieje już w bazie.");
                return View(model);
            }

            var hasher = new PasswordHasher<object>();
            var newUser = new User(model.Username, model.Mail, hasher.HashPassword(null, model.Password));
            _appDbContext.Users.Add(newUser);
            _appDbContext.SaveChanges();

            return View(model);
        }
    }
}