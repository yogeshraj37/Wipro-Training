using AdvancedMvcFilters.App.Filters;
using AdvancedMvcFilters.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedMvcFilters.App.Controllers;

[ServiceFilter(typeof(CustomAuthenticationFilter))]
public class ProductsController : Controller
{
    private static readonly IReadOnlyList<Product> Products =
    [
        new(1, "Wireless Keyboard", 2199, "Low-profile keyboard for everyday work."),
        new(2, "Noise Cancelling Headphones", 7999, "Comfortable headphones with long battery life."),
        new(3, "USB-C Dock", 4599, "Docking station for laptops and tablets.")
    ];

    public IActionResult Index()
    {
        return View(Products);
    }

    public IActionResult Details(int id)
    {
        var product = Products.FirstOrDefault(item => item.Id == id);
        return product is null ? NotFound() : View(product);
    }
}
