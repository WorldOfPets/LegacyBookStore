using LegacyBookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace LegacyBookStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Простой seed для демонстрации
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin User", Email = "admin@example.com", Role = "Admin" },
                new User { Id = 2, Name = "Regular User", Email = "user@example.com", Role = "User" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Clean Code", Author = "Robert Martin", Price = 29.99m },
                new Book { Id = 2, Title = "Domain-Driven Design", Author = "Eric Evans", Price = 39.99m }
            );
        }
    }
}
