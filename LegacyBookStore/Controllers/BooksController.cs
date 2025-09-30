using LegacyBookStore.Data;
using LegacyBookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace LegacyBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly AppDbContext _db;

        public BooksController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _db.Books.ToList();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null)
                return NotFound(new { error = "Book not found" });

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (string.IsNullOrWhiteSpace(book?.Title))
            {
                return BadRequest("Title is required");
            }

            _db.Books.Add(book);
            _db.SaveChanges();

            return Content("Book created", "text/plain");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null)
                return NotFound();

            _db.Books.Remove(book);
            _db.SaveChanges();

            return Ok("Deleted");
        }
    }
}
