using LegacyBookStore.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace LegacyBookStore.Tests
{
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private void ResetDatabase()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Seeding.InitializeTestDb(context);
        }

        [Theory]
        [InlineData("test", "<h1>Welcome, test!</h1>")]
        [InlineData("user", "<h1>Welcome, user!</h1>")]
        public async Task GetUsersWelcome_ReturnsWelcomeString(string name, string expectedContent)
        {
            // Arrange
            ResetDatabase();

            // Act
            var response = await _client.GetAsync($"api/user/welcome?name={name}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedContent, content);
        }

        [Fact]
        public async Task GetUsersWelcomeWithoutName_ReturnsValidationError()
        {
            // Arrange
            ResetDatabase();

            // Act
            var response = await _client.GetAsync("api/user/welcome");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ValidationErrorResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(errorResponse);
            Assert.Equal("One or more validation errors occurred.", errorResponse.Title);
            Assert.Equal(400, errorResponse.Status);
            Assert.NotNull(errorResponse.Errors);
            Assert.True(errorResponse.Errors.ContainsKey("name"));
            Assert.Contains("The name field is required.", errorResponse.Errors["name"]);
        }

        public class ValidationErrorResponse
        {
            public string Type { get; set; }
            public string Title { get; set; }
            public int Status { get; set; }
            public Dictionary<string, string[]> Errors { get; set; }
            public string TraceId { get; set; }
        }
    }
}