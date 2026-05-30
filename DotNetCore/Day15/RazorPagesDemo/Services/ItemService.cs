using RazorPagesAssignment.Models;

namespace RazorPagesAssignment.Services
{
    public static class ItemService
    {
        public static List<Item> Items = new()
        {
            new Item { Name = "Laptop" },
            new Item { Name = "Mobile" },
            new Item { Name = "Keyboard" }
        };
    }
}