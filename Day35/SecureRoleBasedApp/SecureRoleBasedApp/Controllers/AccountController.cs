using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureRoleBasedApp.Models;

namespace SecureRoleBasedApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user,
                        model.Password,
                        false,
                        false);

                    if (result.Succeeded)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }

                        return RedirectToAction("UserProfile");
                    }
                }

                ModelState.AddModelError("", "Invalid Login");
            }

            return View(model);
        }

        [Authorize(Roles = "User")]
        public IActionResult UserProfile()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SeedData()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            var admin = await _userManager.FindByNameAsync("admin");

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = "admin"
                };

                await _userManager.CreateAsync(admin, "Admin@123");

                await _userManager.AddToRoleAsync(admin, "Admin");
            }

            var user = await _userManager.FindByNameAsync("user1");

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "user1"
                };

                await _userManager.CreateAsync(user, "User@123");

                await _userManager.AddToRoleAsync(user, "User");
            }

            return Content("Users and Roles Created");
        }
    }
}