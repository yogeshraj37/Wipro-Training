using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyRazorApp.Pages;

public class IndexModel : PageModel
{

    [BindProperty]
    public string Name { get; set; }
    public string Message { get; set; }





    public void OnGet()
    {

    }
    public void OnPost()
    {
        Message = $"Welcome {Name}! Registration Sucessfully";
    }
}
