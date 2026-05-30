using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesApp.Pages;

public class AddItemModel(ItemStore itemStore) : PageModel
{
    [BindProperty]
    public string NewItem { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        itemStore.Add(NewItem);
        return RedirectToPage("/Index");
    }
}
