using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesAssignment.Models;
using RazorPagesAssignment.Services;

namespace RazorPagesAssignment.Pages
{
    public class AddItemModel : PageModel
    {
        [BindProperty]
        public string ItemName { get; set; }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(ItemName))
            {
                ItemService.Items.Add(
                    new Item
                    {
                        Name = ItemName
                    });
            }

            return RedirectToPage("Index");
        }
    }
}