using AdvancedMvcFilters.App.Filters;
using AdvancedMvcFilters.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedMvcFilters.App.Controllers;

[ServiceFilter(typeof(CustomAuthenticationFilter))]
public class OrdersController : Controller
{
    public IActionResult Index()
    {
        var orders = new[]
        {
            new Order(1001, "Wireless Keyboard", 2199, "Processing"),
            new Order(1002, "USB-C Dock", 4599, "Shipped")
        };

        return View(orders);
    }

    public IActionResult SimulateError()
    {
        throw new InvalidOperationException("Sample order processing failure for testing the global exception filter.");
    }
}
