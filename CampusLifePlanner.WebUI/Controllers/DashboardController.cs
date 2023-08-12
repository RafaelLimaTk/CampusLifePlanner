using Microsoft.AspNetCore.Mvc;

namespace CampusLifePlanner.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
