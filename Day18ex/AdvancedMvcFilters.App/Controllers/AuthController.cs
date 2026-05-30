using Microsoft.AspNetCore.Mvc;

namespace AdvancedMvcFilters.App.Controllers;

public class AuthController : Controller
{
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public IActionResult Login(string userId, string role = "Customer", string? returnUrl = null)
    {
        HttpContext.Session.SetString("UserId", string.IsNullOrWhiteSpace(userId) ? "customer1" : userId);
        HttpContext.Session.SetString("Roles", role);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
