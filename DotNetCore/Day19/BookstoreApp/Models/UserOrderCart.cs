// ==========================================
// Models/AppUser.cs
// ==========================================
using System.ComponentModel.DataAnnotations;

namespace BookstoreApp.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer"; // "Admin" or "Customer"

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}

// ==========================================
// Models/CartItem.cs
// ==========================================
namespace BookstoreApp.Models
{
    public class CartItem
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}

// ==========================================
// Models/Order.cs
// ==========================================
namespace BookstoreApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Shipped, Delivered
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
        public string ShippingAddress { get; set; } = string.Empty;
    }

    public class OrderItem
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}
