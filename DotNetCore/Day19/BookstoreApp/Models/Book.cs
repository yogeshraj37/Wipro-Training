// ==========================================
// Models/Book.cs
// ==========================================
using System.ComponentModel.DataAnnotations;
using BookstoreApp.Attributes;

namespace BookstoreApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required.")]
        [ValidIsbn(ErrorMessage = "ISBN must be in valid ISBN-10 or ISBN-13 format.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [ValidBookPrice(Min = 0.99, Max = 9999.99, ErrorMessage = "Price must be between ₹0.99 and ₹9999.99.")]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = string.Empty;

        [Range(0, 10000, ErrorMessage = "Stock quantity must be between 0 and 10000.")]
        public int StockQuantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsAvailable => StockQuantity > 0;
    }
}
