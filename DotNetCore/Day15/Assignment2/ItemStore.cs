public sealed class ItemStore
{
    public List<string> Items { get; } = new()
    {
        "ASP.NET Core Middleware",
        "Razor Pages"
    };

    public void Add(string item)
    {
        if (!string.IsNullOrWhiteSpace(item))
        {
            Items.Add(item.Trim());
        }
    }
}
