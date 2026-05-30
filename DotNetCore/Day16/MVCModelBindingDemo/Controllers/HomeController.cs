using Microsoft.AspNetCore.Mvc;
using MVCModelBindingDemo.Models;

namespace MVCModelBindingDemo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(Person person)
        {
            return View("Result", person);
        }
    }
}