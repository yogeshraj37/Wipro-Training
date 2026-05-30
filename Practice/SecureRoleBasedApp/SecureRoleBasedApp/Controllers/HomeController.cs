using Microsoft.AspNetCore.Mvc;

namespace SecureRoleBasedApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}