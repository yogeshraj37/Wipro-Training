using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesAssignment.Models;
using RazorPagesAssignment.Services;

namespace RazorPagesAssignment.Pages
{
    public class IndexModel : PageModel
    {
        public List<Item> Items { get; set; }

        public void OnGet()
        {
            Items = ItemService.Items;
        }
    }
}