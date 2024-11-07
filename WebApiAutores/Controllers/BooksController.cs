using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController(ApplicationDBContext context) : ControllerBase
    {
        // [HttpGet("{id:int}")]
        // public async Task<ActionResult<Book>> Get(int id)
        // {
        //     var book = await context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        //     if (book == null) return NotFound();

        //     return Ok(book);
        // }

        // [HttpPost]
        // public async Task<ActionResult<Book>> Post(Book book)
        // {
        //     var existingAuthor = await context.Authors.AnyAsync(a => a.Id == book.AuthorId);
        //     if (!existingAuthor) return BadRequest($"author with id:{book.AuthorId} does not exist");

        //     context.Add(book);
        //     await context.SaveChangesAsync();
        //     return Ok();
        // }
    }
}
