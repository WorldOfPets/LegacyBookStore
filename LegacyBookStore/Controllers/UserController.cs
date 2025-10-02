using LegacyBookStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace LegacyBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }

        [HttpGet("welcome")]
        public IActionResult Welcome(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                name = "Guest";

            var encodedData = HtmlEncoder.Default.Encode(name);

            return Content($"<h1>Welcome, {encodedData}!</h1>", "text/html");
        }
    }
}
