using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kliker.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        public IActionResult Dashboard() // returns main page (like Index.cshtml) for signed in user
        {
            return View();
        }
    }
}
