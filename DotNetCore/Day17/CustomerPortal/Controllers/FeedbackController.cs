using Microsoft.AspNetCore.Mvc;
using CustomerPortal.Models;

namespace CustomerPortal.Controllers
{
    public class FeedbackController : Controller
    {
        private static List<Feedback> feedbacks = new();

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedbacks.Add(feedback);
                return RedirectToAction("List");
            }

            return View(feedback);
        }

        public IActionResult List()
        {
            return View(feedbacks);
        }
    }
}