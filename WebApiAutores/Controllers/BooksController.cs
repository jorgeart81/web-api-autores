using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet("{id:int}", Name = "getBook")]
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

        [HttpPost(Name = "createBook")]
        public async Task<ActionResult<BookDTO>> Post(CreateBookDTO createBookDTO)
        {
            var authorsId = await context.Authors
                .Where(a => createBookDTO.AuthorsId.Contains(a.Id)).Select(a => a.Id).ToListAsync();

            if (createBookDTO.AuthorsId.Count != authorsId.Count) return BadRequest("One of the submitted authors does not exist");

            var book = mapper.Map<Book>(createBookDTO);

            SortBookAuthors(book);

            context.Add(book);
            await context.SaveChangesAsync();

            var bookDTO = mapper.Map<BookDTO>(book);

            return CreatedAtRoute("getBook", new { id = book.Id }, bookDTO);
        }

        [HttpPut("{id:int}", Name = "updateBook")]
        public async Task<ActionResult> Put(int id, CreateBookDTO createBookDTO)
        {
            var bookDB = await context.Books
                .Include(b => b.AuthorsBooks)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bookDB == null) return NotFound();

            bookDB = mapper.Map(createBookDTO, bookDB);

            SortBookAuthors(bookDB);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "patchBook")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PatchBookDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();

            var bookDB = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (bookDB == null) return NotFound();

            var bookDTO = mapper.Map<PatchBookDTO>(bookDB);

            patchDocument.ApplyTo(bookDTO, ModelState);

            var isValid = TryValidateModel(bookDTO);
            if (!isValid) return BadRequest(ModelState);

            mapper.Map(bookDTO, bookDB);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "deleteBook")]
        public async Task<ActionResult> Delete(int id)
        {
            var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            context.Remove(book);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private void SortBookAuthors(Book book)
        {
            if (book.AuthorsBooks == null) return;

            for (int i = 0; i < book.AuthorsBooks.Count; i++)
            {
                book.AuthorsBooks[i].Order = i;
            }
        }
    }
}
