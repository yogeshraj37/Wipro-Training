 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using  MyApp1.Models;

namespace ValidationDemoApp.Controllers
{
    public class StudentController1 : Controller
    {
        public IActionResult Register()
        {
            return View();
        }


    }
    public IActionresulat : Controllers


  [HttpPost]
    public IActionResult Register(Student model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ViewBag.Message = "Registration Successful";
        return View();
    }