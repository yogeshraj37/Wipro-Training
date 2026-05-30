using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesProductApp.Models;

namespace RazorPagesProductApp.Pages.Products
{
    public class IndexModel : PageModel
    {
        public static List<Product> Products = new();

        [BindProperty]
        public Product Product { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            Products.Add(Product);
            return RedirectToPage();
        }
    }
}