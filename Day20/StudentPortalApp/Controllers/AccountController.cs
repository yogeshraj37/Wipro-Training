
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(string username)  // Storing the userName is session value enable by Program.cs
    {
        // Store username in Session
        HttpContext.Session.SetString("UserName", username);

        return RedirectToAction("Dashboard");
    }




    public IActionResult Dashboard()  // This is reading the session Value;
    {

        var user = HttpContext.Session.GetString("UserName");

        ViewBag.User = user;  // This is the variable used for sharing the varibles and storing the //variables;

        return View();
    }

    public IActionResult SetTheme()
{
    CookieOptions options = new CookieOptions();
    options.Expires = DateTime.Now.AddDays(7);
    Response.Cookies.Append("Theme", "Dark", options);
    return Content("Theme Cookie Created");
}
//Reading Cookie
public IActionResult GetTheme()
{
    var theme = Request.Cookies["Theme"];
    return Content("Current Theme: " + theme);
}

}