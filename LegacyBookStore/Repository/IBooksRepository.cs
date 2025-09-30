using LegacyBookStore.Models;

namespace LegacyBookStore.Repository
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Book>> GetBooks();
        Task CreateBook(Book book);
        Task DeleteBook(int id);
        Task<Book?> GetBook(int id);
    }
}
