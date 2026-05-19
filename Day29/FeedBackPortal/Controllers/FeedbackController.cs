using Microsoft.AspNetCore.Mvc;
using FeedbackPortal.Models;

namespace FeedbackPortal.Controllers
{
    public class FeedbackController : Controller
    {
        [HttpGet]
        public IActionResult Create()  // this method floats the empty form 
        {
            return View(new UserComment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserComment submission)  // It's handle the ///submittted form 
        {
            if (!ModelState.IsValid)
            {
                return View(submission);
            }

            

            return RedirectToAction("Success", submission);
        }

        [HttpGet]
        public IActionResult Success(UserComment submission)
        {
            return View(submission);
        }
    }
}