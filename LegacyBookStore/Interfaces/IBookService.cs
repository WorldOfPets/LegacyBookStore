using LegacyBookStore.Models;

namespace LegacyBookStore.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book> CreateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);
        Task<Book?> GetBookByIdAsync(int id);
    }
}