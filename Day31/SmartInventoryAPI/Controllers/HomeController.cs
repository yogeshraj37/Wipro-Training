using Microsoft.AspNetCore.Mvc;

namespace SmartInventoryAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Welcome to Smart Inventory API");
        }

        public IActionResult About()
        {
            return Content("This is Conventional Routing Example");
        }
    }
}