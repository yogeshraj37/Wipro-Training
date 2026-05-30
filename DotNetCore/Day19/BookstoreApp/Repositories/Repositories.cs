// ==========================================
// Repositories/IBookRepository.cs
// ==========================================
using BookstoreApp.Models;

namespace BookstoreApp.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<IEnumerable<Book>> SearchAsync(string? query, string? category);
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByIsbnAsync(string isbn);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

// ==========================================
// Repositories/BookRepository.cs
// ==========================================
using BookstoreApp.Data;
using BookstoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.Repositories
{
    /// <summary>
    /// Repository pattern for data access — separates persistence logic
    /// from business logic (controllers/pages).
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private readonly BookstoreDbContext _context;

        public BookRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
            => await _context.Books.OrderBy(b => b.Title).ToListAsync();

        public async Task<IEnumerable<Book>> SearchAsync(string? query, string? category)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
                books = books.Where(b =>
                    b.Title.Contains(query) ||
                    b.Author.Contains(query) ||
                    b.ISBN.Contains(query));

            if (!string.IsNullOrWhiteSpace(category))
                books = books.Where(b => b.Category == category);

            return await books.OrderBy(b => b.Title).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
            => await _context.Books.FindAsync(id);

        public async Task<Book?> GetByIsbnAsync(string isbn)
            => await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);

        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
            => await _context.Books.AnyAsync(b => b.Id == id);
    }
}

// ==========================================
// Repositories/IOrderRepository.cs + OrderRepository.cs
// ==========================================
using BookstoreApp.Data;
using BookstoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateStatusAsync(int orderId, string status);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly BookstoreDbContext _context;

        public OrderRepository(BookstoreDbContext context) => _context = context;

        public async Task<IEnumerable<Order>> GetAllAsync()
            => await _context.Orders.OrderByDescending(o => o.PlacedAt).ToListAsync();

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
            => await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.PlacedAt)
                .ToListAsync();

        public async Task<Order?> GetByIdAsync(int id)
            => await _context.Orders.FindAsync(id);

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int orderId, string status)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
