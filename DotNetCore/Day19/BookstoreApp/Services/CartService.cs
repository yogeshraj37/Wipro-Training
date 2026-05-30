using BookstoreApp.Models;
using Newtonsoft.Json;

namespace BookstoreApp.Services
{
    /// <summary>
    /// Session-backed shopping cart service.
    /// Cart data is serialized to JSON and stored in the HTTP session,
    /// ensuring it persists across requests for the duration of the session.
    /// </summary>
    public class CartService
    {
        private const string CartSessionKey = "ShoppingCart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        /// <summary>Retrieves current cart from session or returns empty cart.</summary>
        public List<CartItem> GetCart()
        {
            var json = Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
        }

        /// <summary>Adds a book to the cart. If already present, increments quantity.</summary>
        public void AddItem(CartItem newItem)
        {
            var cart = GetCart();
            var existing = cart.FirstOrDefault(i => i.BookId == newItem.BookId);

            if (existing != null)
                existing.Quantity += newItem.Quantity;
            else
                cart.Add(newItem);

            SaveCart(cart);
        }

        /// <summary>Updates the quantity of a cart item. Removes it if quantity <= 0.</summary>
        public void UpdateQuantity(int bookId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.BookId == bookId);

            if (item != null)
            {
                if (quantity <= 0)
                    cart.Remove(item);
                else
                    item.Quantity = quantity;
            }

            SaveCart(cart);
        }

        /// <summary>Removes a specific book from the cart.</summary>
        public void RemoveItem(int bookId)
        {
            var cart = GetCart();
            cart.RemoveAll(i => i.BookId == bookId);
            SaveCart(cart);
        }

        /// <summary>Clears all items from the cart session.</summary>
        public void ClearCart()
        {
            Session.Remove(CartSessionKey);
        }

        /// <summary>Returns total number of items across all cart entries.</summary>
        public int GetTotalItemCount()
        {
            return GetCart().Sum(i => i.Quantity);
        }

        /// <summary>Returns total price of all cart items.</summary>
        public decimal GetTotalPrice()
        {
            return GetCart().Sum(i => i.Subtotal);
        }

        private void SaveCart(List<CartItem> cart)
        {
            Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
        }
    }
}
