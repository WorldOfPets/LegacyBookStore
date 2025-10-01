using LegacyBookStore;
using LegacyBookStore.Data;
using LegacyBookStore.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace LegacyBookStore.Tests;

public class BooksControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BooksControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBooks_ReturnsOkWithListOfBooks()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.EnsureCreated();

        Seeding.RemoveData(context);
        Seeding.InitializeTestDb(context); 

        var response = await _client.GetAsync("api/books");
        response.EnsureSuccessStatusCode();

        var books = await response.Content.ReadFromJsonAsync<List<Book>>();
        Assert.NotNull(books);
        Assert.Equal(2, books.Count);
        Assert.All(books, book =>
        {
            Assert.NotNull(book.Title);
            Assert.NotNull(book.Author);
            Assert.True(book.Price > 0);
        });
    }

    [Fact]
    public async Task GetBooks_ReturnsBook_WhenOneBookExists()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.EnsureCreated();
        Seeding.RemoveData(context);

        var testBook = new Book
        {
            Title = "Integration Test Book",
            Author = "Test Author",
            Price = 12.99m,
            Description = "A book for testing"
        };
        context.Books.Add(testBook);
        await context.SaveChangesAsync();

        var response = await _client.GetAsync("/api/books");
        response.EnsureSuccessStatusCode();

        var books = await response.Content.ReadFromJsonAsync<List<Book>>();

        Assert.NotNull(books);
        Assert.Single(books);
        var book = books[0];
        Assert.Equal("Integration Test Book", book.Title);
        Assert.Equal("Test Author", book.Author);
        Assert.Equal(12.99m, book.Price);
    }
}
