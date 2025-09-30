using LegacyBookStore.Data;
using LegacyBookStore.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddControllers();
var app = builder.Build();


app.Use(async (context, next) =>
{
    var userIdHeader = context.Request.Headers["X-User-Id"].ToString();
    if (!string.IsNullOrEmpty(userIdHeader) && int.TryParse(userIdHeader, out var userId))
    {
        // Нет проверки существования пользователя!
        context.Items["UserId"] = userId;
    }
    await next();
});

app.MapGet("/api/books", (HttpContext context) =>
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var books = db.Books.ToList();
    var json = System.Text.Json.JsonSerializer.Serialize(books);
    context.Response.ContentType = "application/json";
    context.Response.WriteAsync(json);
});

app.MapPost("/api/books", async (HttpContext context) =>
{
    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var book = System.Text.Json.JsonSerializer.Deserialize<LegacyBookStore.Models.Book>(body);

    if (string.IsNullOrWhiteSpace(book?.Title))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Title is required");
        return;
    }

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Books.Add(book);
    await db.SaveChangesAsync();

    context.Response.StatusCode = 201;
    await context.Response.WriteAsync("Created via minimal API (legacy style)");
});
app.MapControllers();

app.Run();
