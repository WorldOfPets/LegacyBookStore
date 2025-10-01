using LegacyBookStore;
using LegacyBookStore.Data;
using LegacyBookStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace LegacyBookStore.Tests;

public class BooksControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BooksControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetBooks_ReturnsOkWithListOfBooks()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Books.RemoveRange(context.Books);
        await context.SaveChangesAsync();

        var testBook = new Book
        {
            Title = "Integration Test Book",
            Author = "Test Author",
            Price = 12.99m,
            Description = "A book for testing"
        };
        context.Books.Add(testBook);
        await context.SaveChangesAsync();
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/books"); 

        // Assert
        response.EnsureSuccessStatusCode();

        var books = await response.Content.ReadFromJsonAsync<List<Book>>();
        Assert.NotNull(books);
        Assert.NotEmpty(books);
        Assert.All(books, book =>
        {
            Assert.NotNull(book.Id);
            Assert.NotNull(book.Title);
            Assert.NotNull(book.Author);
            Assert.True(book.Price > 0);
        });
    }

    [Fact]
    public async Task GetBooks_ReturnsBook_WhenOneBookExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Books.RemoveRange(context.Books);
        await context.SaveChangesAsync();

        var testBook = new Book
        {
            Title = "Integration Test Book",
            Author = "Test Author",
            Price = 12.99m,
            Description = "A book for testing"
        };
        context.Books.Add(testBook);
        await context.SaveChangesAsync();

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/books");
        response.EnsureSuccessStatusCode();

        var books = await response.Content.ReadFromJsonAsync<List<Book>>();

        // Assert
        Assert.NotNull(books);
        Assert.Single(books);
        var book = books[0];
        Assert.Equal("Integration Test Book", book.Title);
        Assert.Equal("Test Author", book.Author);
        Assert.Equal(12.99m, book.Price);
    }
}