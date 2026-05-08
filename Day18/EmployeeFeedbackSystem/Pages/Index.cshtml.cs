using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeFeedbackSystem.Pages;

public class IndexModel : PageModel
{
    [BindProperty]//
    public string EmployeeName { get; set; }


    [BindProperty]// help us in two Way databinding;   
    [Required(ErrorMessage = "Please write Something")]
    public string Feedback { get; set; }
    public string Message { get; set; }
    public void OnGet()
    {

    }

    public void OnPost()
    {
        if (!ModelState.IsValid)
        {
            return;
        }
        Message = $"Thank you \n  Employee Name is {EmployeeName} \n {Feedback}:";
    }
}
