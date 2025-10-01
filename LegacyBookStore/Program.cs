using LegacyBookStore.Data;
using LegacyBookStore.Interfaces;
using LegacyBookStore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookService, BookService>();

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

app.MapControllers();
app.Run();