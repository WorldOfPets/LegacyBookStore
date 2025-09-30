using LegacyBookStore.Data;
using LegacyBookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace LegacyBookStore.Repository
{
    public class BooksRepository : IBooksRepository
    {
        private readonly AppDbContext _db;

        public BooksRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task CreateBook(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteBook(int id)
        {
            var bookForRemove = await GetBook(id);

            if (bookForRemove == null) return;

            _db.Books.Remove(bookForRemove);
            await _db.SaveChangesAsync();

        }

        public async Task<Book?> GetBook(int id)
        {
            return await _db.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _db.Books.ToListAsync();
        }
    }
}
