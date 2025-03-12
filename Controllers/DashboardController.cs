using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kliker.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
