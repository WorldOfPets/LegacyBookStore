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

        public async Task<Book> CreateBook(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var bookForRemove = await GetBook(id);

            if (bookForRemove == null) return false;

            _db.Books.Remove(bookForRemove);
            await _db.SaveChangesAsync();
            return true;

        }

        public async Task<Book?> GetBook(int id)
        {
            return await _db.Books.FindAsync(id);
        }

        public async Task<List<Book>> GetBooks()
        {
            return await _db.Books.ToListAsync();
        }
    }
}
