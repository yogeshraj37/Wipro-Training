using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesApp.Pages;

public class IndexModel(ItemStore itemStore) : PageModel
{
    public IList<string> Items { get; private set; } = [];

    public void OnGet()
    {
        Items = itemStore.Items.ToList();
    }
}
