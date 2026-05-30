// ==========================================
// Controllers/HomeController.cs
// ==========================================
using BookstoreApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepo;

        public HomeController(IBookRepository bookRepo) => _bookRepo = bookRepo;

        public async Task<IActionResult> Index()
        {
            var books = await _bookRepo.GetAllAsync();
            return View(books.Take(6)); // Featured books on homepage
        }

        public IActionResult Error(string? message)
        {
            ViewBag.ErrorMessage = message ?? "An error occurred.";
            return View();
        }
    }
}

// ==========================================
// Controllers/BooksController.cs
// (MVC: list view, detail view, search)
// ==========================================
using BookstoreApp.Repositories;
using BookstoreApp.Services;
using BookstoreApp.Models;
using BookstoreApp.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.Controllers
{
    // Custom route: /catalog/{action}/{id?}
    [Route("catalog/[action]/{id?}")]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepo;
        private readonly CartService _cartService;

        public BooksController(IBookRepository bookRepo, CartService cartService)
        {
            _bookRepo = bookRepo;
            _cartService = cartService;
        }

        // GET /catalog/index
        [HttpGet("/catalog")]
        [HttpGet("/catalog/index")]
        public async Task<IActionResult> Index(string? search, string? category)
        {
            var books = await _bookRepo.SearchAsync(search, category);
            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.CartCount = _cartService.GetTotalItemCount();
            return View(books);
        }

        // GET /catalog/details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null) return NotFound();

            ViewBag.CartCount = _cartService.GetTotalItemCount();
            return View(book);
        }

        // POST /catalog/addtocart — adds book to session cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int bookId, int quantity = 1)
        {
            var book = await _bookRepo.GetByIdAsync(bookId);
            if (book == null || !book.IsAvailable)
            {
                TempData["Error"] = "Book is not available.";
                return RedirectToAction(nameof(Index));
            }

            _cartService.AddItem(new CartItem
            {
                BookId = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Quantity = quantity
            });

            TempData["Success"] = $"'{book.Title}' added to cart!";
            return RedirectToAction(nameof(Details), new { id = bookId });
        }
    }
}

// ==========================================
// Controllers/AdminBooksController.cs
// (MVC Admin: create, edit, delete — Admin only)
// ==========================================
using BookstoreApp.Filters;
using BookstoreApp.Models;
using BookstoreApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.Controllers
{
    [Route("admin/books/[action]/{id?}")]
    [ServiceFilter(typeof(AdminOnlyFilter))] // Entire controller is admin-restricted
    public class AdminBooksController : Controller
    {
        private readonly IBookRepository _bookRepo;

        public AdminBooksController(IBookRepository bookRepo) => _bookRepo = bookRepo;

        // GET /admin/books/index
        [HttpGet("/admin/books")]
        [HttpGet("/admin/books/index")]
        public async Task<IActionResult> Index()
        {
            return View(await _bookRepo.GetAllAsync());
        }

        // GET /admin/books/create
        public IActionResult Create() => View(new Book());

        // POST /admin/books/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid) return View(book);

            // Duplicate ISBN check
            if (await _bookRepo.GetByIsbnAsync(book.ISBN) != null)
            {
                ModelState.AddModelError(nameof(Book.ISBN), "A book with this ISBN already exists.");
                return View(book);
            }

            await _bookRepo.AddAsync(book);
            TempData["Success"] = $"Book '{book.Title}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET /admin/books/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST /admin/books/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id) return BadRequest();
            if (!ModelState.IsValid) return View(book);

            await _bookRepo.UpdateAsync(book);
            TempData["Success"] = $"Book '{book.Title}' updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST /admin/books/delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _bookRepo.ExistsAsync(id)) return NotFound();
            await _bookRepo.DeleteAsync(id);
            TempData["Success"] = "Book deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}

// ==========================================
// Controllers/AuthController.cs
// ==========================================
using BookstoreApp.Data;
using BookstoreApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookstoreApp.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly BookstoreDbContext _context;

        public AuthController(BookstoreDbContext context) => _context = context;

        [HttpGet("/login")]
        public IActionResult Login(string? returnUrl, string? message)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Message = message;
            return View();
        }

        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            var hash = BookstoreDbContext.HashPassword(password);
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == username && u.PasswordHash == hash);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            // Create cookie claims principal
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2) });

            // Also store UserId in session for SessionValidationFilter
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role);

            return Redirect(returnUrl ?? "/");
        }

        [HttpGet("/register")]
        public IActionResult Register() => View();

        [HttpPost("/register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string username, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View();
            }

            if (_context.Users.Any(u => u.Username == username))
            {
                ModelState.AddModelError("", "Username is already taken.");
                return View();
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("", "Email is already registered.");
                return View();
            }

            var user = new AppUser
            {
                Username = username,
                Email = email,
                PasswordHash = BookstoreDbContext.HashPassword(password),
                Role = "Customer"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Registration successful! Please log in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost("/logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

// ==========================================
// Controllers/OrdersController.cs
// ==========================================
using BookstoreApp.Filters;
using BookstoreApp.Models;
using BookstoreApp.Repositories;
using BookstoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.Controllers
{
    [Route("orders/[action]/{id?}")]
    [ServiceFilter(typeof(SessionValidationFilter))] // Requires active session
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IBookRepository _bookRepo;
        private readonly CartService _cartService;

        public OrdersController(IOrderRepository orderRepo, IBookRepository bookRepo, CartService cartService)
        {
            _orderRepo = orderRepo;
            _bookRepo = bookRepo;
            _cartService = cartService;
        }

        // GET /orders/index — user's order history
        [HttpGet("/orders")]
        [HttpGet("/orders/index")]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var orders = await _orderRepo.GetByUserIdAsync(userId);
            return View(orders);
        }

        // GET /orders/checkout — show cart summary before placing order
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.CartTotal = _cartService.GetTotalPrice();
            return View(cart);
        }

        // POST /orders/placeorder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string shippingAddress)
        {
            var cart = _cartService.GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            if (string.IsNullOrWhiteSpace(shippingAddress))
            {
                TempData["Error"] = "Shipping address is required.";
                return RedirectToAction(nameof(Checkout));
            }

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var username = HttpContext.Session.GetString("Username")!;

            // Reduce stock for each ordered book
            foreach (var item in cart)
            {
                var book = await _bookRepo.GetByIdAsync(item.BookId);
                if (book != null && book.StockQuantity >= item.Quantity)
                {
                    book.StockQuantity -= item.Quantity;
                    await _bookRepo.UpdateAsync(book);
                }
            }

            var order = new Order
            {
                UserId = userId,
                Username = username,
                Items = cart.Select(c => new OrderItem
                {
                    BookId = c.BookId,
                    Title = c.Title,
                    Price = c.Price,
                    Quantity = c.Quantity
                }).ToList(),
                TotalAmount = _cartService.GetTotalPrice(),
                ShippingAddress = shippingAddress,
                Status = "Confirmed"
            };

            await _orderRepo.AddAsync(order);
            _cartService.ClearCart();

            TempData["OrderId"] = order.Id;
            return RedirectToAction(nameof(Confirmation), new { id = order.Id });
        }

        // GET /orders/confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            if (order.UserId != userId && HttpContext.Session.GetString("UserRole") != "Admin")
                return Forbid();

            return View(order);
        }

        // GET /orders/adminall — Admin: all orders
        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("/admin/orders")]
        public async Task<IActionResult> AdminAll()
        {
            return View(await _orderRepo.GetAllAsync());
        }

        // POST /orders/updatestatus — Admin: update order status
        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            await _orderRepo.UpdateStatusAsync(orderId, status);
            TempData["Success"] = $"Order #{orderId} status updated to {status}.";
            return RedirectToAction(nameof(AdminAll));
        }
    }
}
