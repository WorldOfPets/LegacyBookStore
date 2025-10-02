using LegacyBookStore.Interfaces;
using LegacyBookStore.Models;
using LegacyBookStore.Repository;

namespace LegacyBookStore.Services
{
    public class BookService : IBookService
    {
        private readonly IBooksRepository _booksRepository;

        public BookService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _booksRepository.GetBooks();
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            return await _booksRepository.CreateBook(book);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await _booksRepository.DeleteBook(id);
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _booksRepository.GetBook(id);
        }
    }
}