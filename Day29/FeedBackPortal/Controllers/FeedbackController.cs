using Microsoft.AspNetCore.Mvc;
using FeedbackPortal.Models;

namespace FeedbackPortal.Controllers
{
    public class FeedbackController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserComment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserComment submission)
        {
            if (!ModelState.IsValid)
            {
                return View(submission);
            }

            // Save to database here if needed

            return RedirectToAction("Success", submission);
        }

        [HttpGet]
        public IActionResult Success(UserComment submission)
        {
            return View(submission);
        }
    }
}