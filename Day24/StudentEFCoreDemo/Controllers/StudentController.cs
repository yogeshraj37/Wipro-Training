using Microsoft.AspNetCore.Mvc;
using StudentEFCoreDemo.Data;
using StudentEFCoreDemo.Models;

namespace StudentEFCoreDemo.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppdbContext _context;

        public StudentController(AppdbContext context)
        {
            _context = context;
        }

        public IActionResult AddSample()
        {
            var student = new Student
            {
                Name = "Yogesh",
                Age = 22
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            return Content("Student inserted successfully.");
        }

        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            return View(students);
        }
    }
}