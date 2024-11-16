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
        public async Task<ActionResult<BookDTOWithAuthors>> Get(int id)
        {
            var book = await context.Books
                .Include(b => b.AuthorsBooks)
                .ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            book.AuthorsBooks = book.AuthorsBooks.OrderBy(b => b.Order).ToList();

            return Ok(mapper.Map<BookDTOWithAuthors>(book));
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post(CreateBookDTO bookDTO)
        {
            var authorsId = await context.Authors
                .Where(a => bookDTO.AuthorsId.Contains(a.Id)).Select(a => a.Id).ToListAsync();

            if (bookDTO.AuthorsId.Count != authorsId.Count) return BadRequest("One of the submitted authors does not exist");

            var book = mapper.Map<Book>(bookDTO);

            for (int i = 0; i < book.AuthorsBooks.Count; i++)
            {
                book.AuthorsBooks[i].Order = i;
            }

            context.Add(book);
            await context.SaveChangesAsync();

            return Created();
        }
    }
}
