using AdvancedMvcFilters.App.Filters;
using AdvancedMvcFilters.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedMvcFilters.App.Controllers;

[ServiceFilter(typeof(CustomAuthenticationFilter))]
public class BankingController : Controller
{
    public IActionResult Accounts()
    {
        var accounts = new[]
        {
            new BankAccount("SB-1001", "Savings", 84250),
            new BankAccount("CA-2044", "Current", 125000)
        };

        return View(accounts);
    }

    public IActionResult Transactions()
    {
        var transactions = new[]
        {
            new BankTransaction("TRX-901", "Salary credit", 75000, DateTime.Today.AddDays(-5)),
            new BankTransaction("TRX-902", "Electricity bill", -2450, DateTime.Today.AddDays(-2))
        };

        return View(transactions);
    }

    [ServiceFilter(typeof(UserActionLoggingFilter))]
    public IActionResult Transfer()
    {
        return View();
    }

    [TypeFilter(typeof(RoleAuthorizationFilter), Arguments = new object[] { "Admin" })]
    public IActionResult AdminAccounts()
    {
        return View("Accounts", new[] { new BankAccount("ADMIN-001", "All Customer Accounts", 980000) });
    }
}
