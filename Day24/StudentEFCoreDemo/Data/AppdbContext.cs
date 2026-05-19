using Microsoft.EntityFrameworkCore;
using StudentEFCoreDemo.Models;

namespace StudentEFCoreDemo.Data
{
    public class AppdbContext : DbContext
    {
        public AppdbContext(DbContextOptions<AppdbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}