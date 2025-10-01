using LegacyBookStore.Models;

namespace LegacyBookStore.Repository
{
    public interface IBooksRepository
    {
        Task<List<Book>> GetBooks();
        Task<Book> CreateBook(Book book);
        Task<bool> DeleteBook(int id);
        Task<Book?> GetBook(int id);
    }
}
