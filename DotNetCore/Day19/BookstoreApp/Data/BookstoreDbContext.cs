using BookstoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.Data
{
    public class BookstoreDbContext : DbContext
    {
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Owned entity for OrderItem (stored as JSON in EF Core 8)
            modelBuilder.Entity<Order>().OwnsMany(o => o.Items, i =>
            {
                i.WithOwner();
            });

            // Seed sample books
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", ISBN = "9780132350884", Price = 499.00m, Category = "Programming", StockQuantity = 15, Description = "A handbook of agile software craftsmanship." },
                new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "David Thomas, Andrew Hunt", ISBN = "9780201616224", Price = 599.00m, Category = "Programming", StockQuantity = 10, Description = "Your journey to mastery." },
                new Book { Id = 3, Title = "Design Patterns", Author = "Gang of Four", ISBN = "9780201633610", Price = 749.00m, Category = "Architecture", StockQuantity = 8, Description = "Elements of reusable object-oriented software." },
                new Book { Id = 4, Title = "The Alchemist", Author = "Paulo Coelho", ISBN = "9780062315007", Price = 199.00m, Category = "Fiction", StockQuantity = 25, Description = "A magical story about following your dreams." },
                new Book { Id = 5, Title = "Atomic Habits", Author = "James Clear", ISBN = "9780735211292", Price = 349.00m, Category = "Self-Help", StockQuantity = 20, Description = "Tiny changes, remarkable results." }
            );

            // Seed admin and a sample customer
            // Passwords stored as simple hash (BCrypt recommended for production)
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = 1, Username = "admin", Email = "admin@bookstore.com", PasswordHash = HashPassword("Admin@123"), Role = "Admin" },
                new AppUser { Id = 2, Username = "john_doe", Email = "john@example.com", PasswordHash = HashPassword("John@123"), Role = "Customer" }
            );
        }

        // Simple SHA256 hash — use BCrypt in production
        public static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password + "BookstoreSalt2024");
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }
}
