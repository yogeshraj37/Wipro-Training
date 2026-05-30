using BookstoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.Controllers
{
    [Route("cart/[action]")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService) => _cartService = cartService;

        // GET /cart
        [HttpGet("/cart")]
        [HttpGet("/cart/index")]
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            ViewBag.Total = _cartService.GetTotalPrice();
            return View(cart);
        }

        // POST /cart/remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int bookId)
        {
            _cartService.RemoveItem(bookId);
            TempData["Success"] = "Item removed from cart.";
            return RedirectToAction(nameof(Index));
        }

        // POST /cart/update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int bookId, int quantity)
        {
            if (quantity < 0) quantity = 0;
            _cartService.UpdateQuantity(bookId, quantity);
            return RedirectToAction(nameof(Index));
        }

        // POST /cart/clear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            _cartService.ClearCart();
            TempData["Success"] = "Cart cleared.";
            return RedirectToAction(nameof(Index));
        }
    }
}
