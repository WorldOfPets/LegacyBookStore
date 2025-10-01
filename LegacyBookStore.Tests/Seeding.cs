using LegacyBookStore.Data;
using LegacyBookStore.Models;
using System.Threading.Tasks;

namespace LegacyBookStore.Tests
{
    public class Seeding
    {
        public static void InitializeTestDb(AppDbContext db)
        {

            db.Books.AddRange(
                new Book { Title = "Clean Code", Author = "Robert Martin", Price = 29.99m },
                new Book { Title = "Domain-Driven Design", Author = "Eric Evans", Price = 39.99m }
            );
            db.SaveChanges();
        }

        public static void RemoveData(AppDbContext db) 
        {
            db.Books?.RemoveRange(db.Books);
            db.Users?.RemoveRange(db.Users);
            db.SaveChanges();
        }
    }
}
