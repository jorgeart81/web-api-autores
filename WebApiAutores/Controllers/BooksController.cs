using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController(ApplicationDBContext context, IMapper mapper) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDTO>> Get(int id)
        {
            var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            return Ok(mapper.Map<BookDTO>(book));
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post(CreateBookDTO bookDTO)
        {
            // var existingAuthor = await context.Authors.AnyAsync(a => a.Id == book.AuthorId);
            // if (!existingAuthor) return BadRequest($"author with id:{book.AuthorId} does not exist");
            var book = mapper.Map<Book>(bookDTO);

            context.Add(book);
            await context.SaveChangesAsync();

            return Created();
        }
    }
}
